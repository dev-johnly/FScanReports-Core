using FScan.Reports.Application.Contracts;
using FScan.Reports.Application.Models.DTOs;
using FScan.Reports.Application.Models.Requests;
using FScan.Reports.Application.Models.ViewModels;
using FScan.Reports.Domain.Entities;
using FScan.Reports.Infrastructure.Data;
using FScan.Reports.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Infrastructure.Repositories;

public class ReportsRepository : IReportsRepository
{
    private readonly UserClaimsGetter _userClaims;
    private readonly FScanContext _context;
    private readonly string? _ID;

    public ReportsRepository(UserClaimsGetter userClaims, FScanContext context)
    {
        _userClaims = userClaims;
        _context = context;
        _ID = _userClaims.GetClaims().ID;
    }

    public async Task<TimeSheetDTO> TimeSheetsAsync()
    {
        TimeSheetDTO ts = new();

        var report = await _context.NGAC_USERINFO.FirstOrDefaultAsync(s => s.ID == _ID);

        var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
        var attList = await GetTimesheet(firstDayOfMonth, lastDayOfMonth);

        ts.EmpId = report.ID;
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

    public async Task <List<AttendanceDTO>> ProcessAttendance(List<AttendanceDTO> attList)
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

}









