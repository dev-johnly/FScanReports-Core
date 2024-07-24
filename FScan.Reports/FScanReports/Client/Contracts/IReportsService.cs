using FScan.Reports.Application.Models.DTOs;
using FScan.Reports.Application.Models.Requests;

namespace FScanReports.Client.Contracts;

public interface IReportsService
{
    Task<TimeSheetDTO> TimeSheetsAsync();
    Task<TimeSheetDTO> GetFilteredTimeSheetAsync(DateFilterRequest request);
}
