using FScan.Reports.Application.Contracts;
using FScan.Reports.Application.Models.DTOs;
using FScan.Reports.Application.Models.Helpers;
using FScan.Reports.Application.Models.Requests;
using FScan.Reports.Application.Models.Responses;
using FScan.Reports.Infrastructure.Data;
using FScan.Reports.Infrastructure.Helpers;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;


namespace FScan.Reports.Infrastructure.Repositories;

public class ReportsRepository : IReportsRepository
{
    private readonly UserClaimsGetter _userClaims;
    private readonly FScanContext _context;
    private readonly string? _ID;
    private readonly IHostingEnvironment _environment;

    public ReportsRepository(FScanContext context, IHttpContextAccessor httpContextAccessor, IHostingEnvironment environment)
    {
        //_userClaims = userClaims;
        _userClaims = new UserClaimsGetter(httpContextAccessor);
        _context = context;
        _ID = _userClaims.GetClaims().ID;
       _environment = environment;
    }

    public async Task<TimeSheetDTO> TimeSheetsAsync()
    {
        TimeSheetDTO ts = new();

        var report = await _context.NGAC_USERINFO.FirstOrDefaultAsync(s => s.ID == _ID);

        var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
        var attList = await GetTimesheet(firstDayOfMonth, lastDayOfMonth);

        ts.EmpId = _ID;
        ts.EmpName = report.Name;
        ts.DateFrom = firstDayOfMonth;
        ts.DateTo = lastDayOfMonth;
        ts.AttendanceList = await ProcessAttendance(attList);

        return ts;


    }

    public async Task<TimeSheetDTO> GetFilteredTimeSheetAsync(DateFilterRequest request)
    {
        try
        {
            TimeSheetDTO response = new();
            response.DateFrom = request.DateFrom.GetValueOrDefault();
            response.DateTo = request.DateTo.GetValueOrDefault();

            var attList = await GetTimesheet(request.DateFrom.GetValueOrDefault(), request.DateTo.GetValueOrDefault());

            response.AttendanceList = await ProcessAttendance(attList);

            return response;
        }
        catch (Exception ex)
        {
            return new TimeSheetDTO();
        }


    }

    public async Task<List<AttendanceDTO>> GetTimesheet(DateTime dateFrom, DateTime dateTo)
    {
        string? id = _ID;
        if (id == null) return new List<AttendanceDTO>();

        var endDate = dateTo.AddDays(1);
        try
        {
            var report = await _context.NGAC_AUTHLOG
                .Where(log => log.UserID == id &&
                   log.TransactionTime >= dateFrom &&
                   log.TransactionTime < endDate)
                .ToListAsync();


            var groupedReport = report
          .GroupBy(s => s.TransactionTime.Value.Date)
          .ToDictionary(g => g.Key, g => g.ToList());

            var attendanceList = new List<AttendanceDTO>();

            for (var date = dateFrom.Date; date <= dateTo.Date; date = date.AddDays(1))
            {
                var att = new AttendanceDTO
                {
                    AttendanceDate = date,
                    TimeIn = groupedReport.TryGetValue(date, out var dayRecords) && dayRecords.Any(r => r.FunctionKey == 1)
                        ? dayRecords.First(r => r.FunctionKey == 1).TransactionTime.Value
                        : DateTime.MinValue,
                    LunchOut = groupedReport.TryGetValue(date, out dayRecords) && dayRecords.Any(r => r.FunctionKey == 2)
                        ? dayRecords.Last(r => r.FunctionKey == 2).TransactionTime.Value
                        : DateTime.MinValue,
                    LunchIn = groupedReport.TryGetValue(date, out dayRecords) && dayRecords.Any(r => r.FunctionKey == 3)
                        ? dayRecords.Last(r => r.FunctionKey == 3).TransactionTime.Value
                        : DateTime.MinValue,
                    TimeOut = groupedReport.TryGetValue(date, out dayRecords) && dayRecords.Any(r => r.FunctionKey == 4)
                        ? dayRecords.Last(r => r.FunctionKey == 4).TransactionTime.Value
                        : DateTime.MinValue
                };

                attendanceList.Add(att);
            }

            return attendanceList;
        }
        catch (Exception e)
        {
            return new List<AttendanceDTO> { new AttendanceDTO() };
        }


    }

    public async Task<List<AttendanceDTO>> ProcessAttendance(List<AttendanceDTO> attList)
    {
        List<AttendanceDTO> attendanceDTOList = new List<AttendanceDTO>();

        AttendanceDTO att = null;

        foreach (var item in attList)
        {
            att = new AttendanceDTO();
            att.AttendanceDateText = GetDateAndDay(item.AttendanceDate.GetValueOrDefault());
            att.LunchInText = item.LunchIn == DateTime.MinValue ? "No In" : item.LunchIn.GetValueOrDefault().ToString("HH:mm:ss");
            att.LunchOutText = item.LunchOut == DateTime.MinValue ? "No Out" : item.LunchOut.GetValueOrDefault().ToString("HH:mm:ss");
            att.TimeInText = item.TimeIn == DateTime.MinValue ? "No In" : item.TimeIn.GetValueOrDefault().ToString("HH:mm:ss");
            att.TimeOutText = item.TimeOut == DateTime.MinValue ? "No Out" : item.TimeOut.GetValueOrDefault().ToString("HH:mm:ss");

            if (item.TimeOut != DateTime.MinValue && item.TimeIn != DateTime.MinValue)
            {
                att.TotalTime = item.TimeOut.GetValueOrDefault().Subtract(item.TimeIn.GetValueOrDefault());
            }
            else
            {
                att.TotalTime = TimeSpan.Zero;
            }

            if (item.LunchIn != DateTime.MinValue && item.LunchOut != DateTime.MinValue)
            {
                att.TotalLunchTime = item.LunchIn.GetValueOrDefault().Subtract(item.LunchOut.GetValueOrDefault());
            }
            else
            {
                att.TotalLunchTime = TimeSpan.Zero;
            }

            attendanceDTOList.Add(att);
        }

        return attendanceDTOList;
    }

    private string GetDateAndDay(DateTime attendanceDate)
    {
        string day = string.Empty;

        switch ((int)attendanceDate.DayOfWeek)
        {
            case 0:
                day = "Sunday";
                break;
            case 1:
                day = "Monday";
                break;
            case 2:
                day = "Tuesday";
                break;
            case 3:
                day = "Wednesday";
                break;
            case 4:
                day = "Thursday";
                break;
            case 5:
                day = "Friday";
                break;
            case 6:
                day = "Saturday";
                break;
        }

        day = string.Format("{0}, {1}", attendanceDate.ToShortDateString(), day);

        return day;
    }

    public async Task<PagedResult<FSLogsDTO>> FScanLogsAsync(string? key, int currentPage, int pageSize)
    {
        try
        {

            var usercode = _ID;
            var query = from A in _context.NGAC_USERINFO
                        join B in _context.NGAC_GROUP on A.GroupID equals B.ID into grouptb
                        from grp in grouptb.DefaultIfEmpty()
                        where A.Name != "admin"
                        select new FSLogsDTO
                        {
                            IndexKey = A.IndexKey,
                            ID = A.ID,
                            Name = A.Name ?? "",
                            GroupName = grp.Name,
                            AccessType = A.AccessType ?? ""
                        };



            if (!string.IsNullOrEmpty(key))
            {
                var trimmedKey = key.Trim();
                query = query.Where(x => x.IndexKey.ToString().Contains(trimmedKey) ||
                                         x.ID.Contains(trimmedKey) ||
                                         x.Name.Contains(trimmedKey) ||
                                         x.GroupName.Contains(trimmedKey) ||
                                         x.AccessType.Contains(trimmedKey) ||
                                         x.GroupName.Contains(trimmedKey)

                                         );
            }

            var count = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)count / pageSize);

            var items = await query.OrderBy(x => x.IndexKey)
                                   .Skip((currentPage - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return new PagedResult<FSLogsDTO>
            {
                Items = items,
                TotalPages = totalPages,
                TotalCount = count
            };
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while fetching paged results", ex);
        }
    }

    public async Task<FSLogsDetailsResponse> FScanLogsDetailsAsync(FSLogDetailsRequest request)
    {
        try
        {
            //FSLogsDetailsDTO Sheet = new();
            FSLogsDetailsResponse response = new();

            if (request.DateFrom.GetValueOrDefault().ToString("yyyy") == "0001" || request.DateTo.GetValueOrDefault().ToString("yyyy") == "0001")
            {
                request.DateFrom = DateTime.Now;
                request.DateTo = DateTime.Now;
            }

            string USERIDINDEX = "";

            var dft = string.Empty;
            var Logs = new List<FSLogsDetailsDTO>();
            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            //Sheet.DateFrom = request.DateFrom.GetValueOrDefault();
            //Sheet.DateTo = request.DateTo.GetValueOrDefault();

            var result = await (from A in _context.NGAC_USERINFO
                                join B in _context.NGAC_GROUP on A.GroupID equals B.ID into grouptb
                                from grp in grouptb.DefaultIfEmpty()
                                from C in _context.NGAC_AUTHLOG
                                from D in _context.NGAC_TERMINAL
                                    //where A.GroupID == grp.ID
                                where C.UserID == A.ID
                                where D.ID == C.TerminalID
                                where A.ID == request.ID
                                where C.TransactionTime.Value.DayOfYear >= request.DateFrom.GetValueOrDefault().DayOfYear
                                where C.TransactionTime.Value.DayOfYear <= request.DateTo.GetValueOrDefault().DayOfYear

                                select new FSLogsDetailsDTO
                                {
                                    Date = C.TransactionTime,
                                    ID = A.ID,
                                    Name = A.Name,
                                    GroupName = grp.Name,
                                    TerminalName = D.Name,
                                    FunctionKey = C.FunctionKey
                                }).ToListAsync();
            foreach (var item in result)
            {
                var i = new FSLogsDetailsDTO();
                i.Date = item.Date;
                i.ID = item.ID;
                i.Name = item.Name;
                i.GroupName = item.GroupName;
                i.TerminalName = item.TerminalName;
                i.FunctionKey = item.FunctionKey;


                response.DetailsName = item.Name;
                response.DateFrom = request.DateFrom;
                response.DateTo = request.DateTo;
                response.DetailsId = item.ID;
                response.GroupName = item.GroupName;
                USERIDINDEX = item.ID;

                //string test = ViewData["DateFrom"].ToString();



                Logs.Add(i);
            }
            //response.GetLogsList = Logs;
            if (result.Count() == 0)
            {

                var result2 = await (from A in _context.NGAC_USERINFO
                                     join B in _context.NGAC_GROUP on A.GroupID equals B.ID into grouptb
                                     from grp in grouptb.DefaultIfEmpty()
                                     from C in _context.NGAC_AUTHLOG
                                     from D in _context.NGAC_TERMINAL
                                         //where A.GroupID == grp.ID
                                     where C.UserID == A.ID
                                     where D.ID == C.TerminalID
                                     where A.ID == request.ID

                                     select new FSLogsDetailsDTO
                                     {
                                         Date = C.TransactionTime,
                                         ID = A.ID,
                                         Name = A.Name,
                                         GroupName = grp.Name,
                                         TerminalName = D.Name,
                                         FunctionKey = C.FunctionKey
                                     }).ToListAsync();

                foreach (var item in result2)
                {
                    var i = new FSLogsDetailsDTO();
                    i.Date = item.Date;
                    i.ID = item.ID;
                    i.Name = item.Name;
                    i.GroupName = item.GroupName;
                    i.TerminalName = item.TerminalName;
                    i.FunctionKey = item.FunctionKey;
                    response.DetailsName = item.Name;
                    response.DateFrom = request.DateFrom;
                    response.DateTo = request.DateTo;
                    response.DetailsId = item.ID;
                    response.GroupName = item.GroupName;
                    USERIDINDEX = item.ID;



                    Logs.Add(i);
                }

                if (request.Set)
                {
                    response.GetLogs = dft;
                    response.DateFrom = request.DateFrom;
                    response.DateTo = request.DateTo;

                }
                else
                {
                    response.GetLogs = dft;
                    response.DateFrom = firstDayOfMonth;
                    response.DateTo = lastDayOfMonth;
                }


            }
            else
            {
                if (request.Set)
                {
                    response.GetLogsList = Logs;
                }
                else
                {
                    response.GetLogs = dft;
                    response.DateFrom = firstDayOfMonth;
                    response.DateTo = lastDayOfMonth;
                }
            }

            return response;
        }
        catch (Exception ex)
        {
            return new FSLogsDetailsResponse();
        }
    }

    private List<FSLogsDetailsDTO> Logdetails(List<FSLogsDetailsDTO> attList)
    {
        List<FSLogsDetailsDTO> LogsDetails = new List<FSLogsDetailsDTO>();

        FSLogsDetailsDTO att = null;

        foreach (var item in attList)
        {
            att = new FSLogsDetailsDTO();
            att.Date = item.Date;
            att.ID = item.ID;
            att.Name = item.Name;
            att.GroupName = item.GroupName;
            att.TerminalName = item.TerminalName;
            att.FunctionKey = item.FunctionKey;


            LogsDetails.Add(att);
        }

        return LogsDetails;
    }

    public async Task<PdfResponse> ExportToPDF(PdfRequest sheet)
    {
        var EmployeeId = _ID;
        if (!string.IsNullOrEmpty(EmployeeId))
        {
            try {
                List<AttendanceDTO> attList = await GetTimesheet(sheet.DateFrom.GetValueOrDefault(), sheet.DateTo.GetValueOrDefault());
                var AttendanceList = await ProcessAttendance(attList);

                //string filename = string.Format("AttendanceReport{0}.pdf", DateTime.Now.Date.ToString("yyyyMMdd"));
                string filename = $"AttendanceReport_{Guid.NewGuid()}.pdf";
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "TimesheetDump");

                // Ensure the directory exists
                Directory.CreateDirectory(folderPath);

                // Combine folder path with filename
                string fullPath = Path.Combine(folderPath, filename);

                //filename = Path.Combine(Directory.GetCurrentDirectory(), @"TimesheetDump", filename);
                BuildInquireReportPDF(AttendanceList, fullPath, EmployeeId, sheet.DateFrom.GetValueOrDefault(), sheet.DateTo.GetValueOrDefault());

                // Read the file into a byte array
                byte[] fileBytes = await File.ReadAllBytesAsync(fullPath);


                return new PdfResponse
                {
                    Filename = fullPath,
                    FileBytes = fileBytes
                };
            }
            catch (Exception ex) {
                return new PdfResponse
                {
                    Filename = "",
                    ErrorMessage = ex.Message

                };
            }
          
        }
        else
        {
            return new PdfResponse
            {
                Filename = "",

            };
        }
      
    }

    private void BuildInquireReportPDF(List<AttendanceDTO> attList, string filename, string EmployeeId /*Nullable*/, DateTime DateFrom, DateTime DateTo)
    {
        try {
            PDFUtil pdf = new PDFUtil(_environment);

            float[] headerSize = { 50, 35, 35, 35, 35, 35, 35 };
            PdfPTable content = new PdfPTable(7);

            content = pdf.AddCellToBody(content, headerSize, "Finger Scan - Attendance Report", 7); // colSpan value
            content = pdf.AddCellToBody(content, headerSize, "Period From " + DateFrom.ToString("MM/dd/yyyy") + " to " + DateTo.ToString("MM/dd/yyyy"), 7);

            string name = "";
            string id = "";
            string Unit = "";

            // Use this statement to generate other DTR Report
            var QUERY = (from a in _context.NGAC_USERINFO
                         where a.ID == EmployeeId
                         select a).ToList();
            foreach (var item in QUERY)
            {
                var QUERY2 = (from a in _context.NGAC_GROUP
                              where a.ID == item.GroupID
                              select a).FirstOrDefault();

                id = item.ID;
                name = item.Name;

                if (QUERY2 != null)
                    Unit = QUERY2.Name;
                else
                    Unit = string.Empty;
            }

            pdf.AddNameHeader(content, headerSize, name + " (" + id + ")");
            pdf.AddNameHeader(content, headerSize, Unit);
            pdf.AddNameHeader(content, headerSize, "");

            pdf.AddCellHeader(content, "", 1);
            pdf.AddCellHeader(content, "ATTENDANCE", 3);
            pdf.AddCellHeader(content, "LUNCH BREAK", 3);

            content = pdf.AddCellToBodySubHeader(content, "DATE");
            content = pdf.AddCellToBodySubHeader(content, "TIME IN (F1)");
            content = pdf.AddCellToBodySubHeader(content, "TIME OUT (F4)");
            content = pdf.AddCellToBodySubHeader(content, "TOTAL HRS");
            content = pdf.AddCellToBodySubHeader(content, "START (F2)");
            content = pdf.AddCellToBodySubHeader(content, "END (F3)");
            content = pdf.AddCellToBodySubHeader(content, "TOTAL HRS");

            foreach (var att in attList)
            {
                content = pdf.AddCellToBodyLeft(content, att.AttendanceDateText);
                content = pdf.AddCellToBodyLeft(content, att.TimeInText);
                content = pdf.AddCellToBodyLeft(content, att.TimeOutText);
                content = pdf.AddCellToBodyLeft(content, att.TotalTime.ToString());
                content = pdf.AddCellToBodyLeft(content, att.LunchOutText);
                content = pdf.AddCellToBodyLeft(content, att.LunchInText);
                content = pdf.AddCellToBodyLeft(content, att.TotalLunchTime.ToString());

            }

            pdf.AddFooterCells(content, headerSize, "");
            pdf.AddFooterCells(content, headerSize, "");
            pdf.AddFooterCells(content, headerSize, "       _________________________               _________________________             _________________________");
            pdf.AddFooterCells(content, headerSize, "Employee Signature                                    Checked By                                           Approved By        ");

            MemoryStream pdfStream = pdf.CreatePdf(content, filename, true, true);
        }
        catch(Exception e) { }
    }

}









