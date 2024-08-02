using FScan.Reports.Application.Models.DTOs;
using FScanReports.Client.Contracts;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text.Json;
using FScan.Reports.Application.Models.Requests;
using FScan.Reports.Application.Models.Helpers;
using FScan.Reports.Application.Models.Responses;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FScanReports.Client.Services;

public class ReportsService : IReportsService
{
    private readonly HttpClient _httpClient;
    public ReportsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<TimeSheetDTO> TimeSheetsAsync()
    {
        var response = await _httpClient.GetAsync("api/Reports/TimeSheets");
        var result = await response.Content.ReadFromJsonAsync<TimeSheetDTO>();
        return result!; 

    }
    public async Task<TimeSheetDTO> GetFilteredTimeSheetAsync(DateFilterRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Reports/GetFilteredTimeSheet", request);

        var result = await response.Content.ReadFromJsonAsync<TimeSheetDTO>();
        return result!;
    }

    public async Task<PagedResult<FSLogsDTO>> FScanLogsAsync(string? key, int currentPage, int pageSize)
    {
        var response = await _httpClient.GetAsync($"api/Reports/FScanLogs?key={key}&currentPage={currentPage}&pageSize={pageSize}");
        var result = await response.Content.ReadFromJsonAsync<PagedResult<FSLogsDTO>>();
        return result!;
    }

    public async Task<FSLogsDetailsResponse> FScanLogsDetailsAsync(FSLogDetailsRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Reports/FScanLogsDetails",request);
        var result = await response.Content.ReadFromJsonAsync<FSLogsDetailsResponse>();
        return result!;
    }


    public async Task<PdfResponse> ExportToPDF(PdfRequest sheet)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Reports/ExportToPDF", sheet);
        var result = await response.Content.ReadFromJsonAsync<PdfResponse>();
        return result!;
    }
}
