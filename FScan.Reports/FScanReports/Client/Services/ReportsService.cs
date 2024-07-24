using FScan.Reports.Application.Models.DTOs;
using FScanReports.Client.Contracts;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text.Json;
using FScan.Reports.Application.Models.Requests;

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
        var response = await _httpClient.GetAsync($"api/Reports/TimeSheets/{null}");
        var result = await response.Content.ReadFromJsonAsync<TimeSheetDTO>();
        return result!; 

    }


    //public async Task<TimeSheetDTO> GetFilteredTimeSheetAsync(TimeSheetDTO request)
    //{
    //    var response = await _httpClient.PostAsJsonAsync("api/Reports/GetFilteredTimeSheet", request);
    //    var result = await response.Content.ReadFromJsonAsync<TimeSheetDTO>();

    //    return result!;
    //}

    public async Task<TimeSheetDTO> GetFilteredTimeSheetAsync(DateFilterRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Reports/GetFilteredTimeSheet", request);

        var result = await response.Content.ReadFromJsonAsync<TimeSheetDTO>();
        return result!;
    }
}
