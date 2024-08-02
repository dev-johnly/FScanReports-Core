using FScan.Reports.Application.Models.DTOs;
using FScan.Reports.Application.Models.Helpers;
using FScan.Reports.Application.Models.Requests;
using FScan.Reports.Application.Models.Responses;

namespace FScanReports.Client.Contracts;

public interface IReportsService
{
    Task<TimeSheetDTO> TimeSheetsAsync();
    Task<TimeSheetDTO> GetFilteredTimeSheetAsync(DateFilterRequest request);
    //Task<List<FSLogsDTO>> FScanLogs();
    Task<PagedResult<FSLogsDTO>> FScanLogsAsync(string? key, int currentPage, int pageSize);

    Task<FSLogsDetailsResponse> FScanLogsDetailsAsync(FSLogDetailsRequest request);

    Task<PdfResponse> ExportToPDF(PdfRequest sheet);
}
