using FScan.Reports.Application.Contracts;
using FScan.Reports.Application.Models.DTOs;
using FScan.Reports.Application.Models.Requests;
using FScan.Reports.Application.Models.Responses;
using FScan.Reports.Application.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace FScanReports.Server.Controller;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IReportsRepository _reports;

    public ReportsController(IReportsRepository reports)
    {
        _reports = reports;

    }

    [HttpGet("TimeSheets")]
    public async Task<IActionResult> TimeSheetsAsync()
    {
        var result = await _reports.TimeSheetsAsync();
        return Ok(result);

    }

    [HttpPost("GetFilteredTimeSheet")]
    public async Task<IActionResult> GetFilteredTimeSheetAsync([FromBody] DateFilterRequest request)
    {
        var result = await _reports.GetFilteredTimeSheetAsync(request);
        return Ok(result);

    }

    [HttpGet("FScanLogs")]
    public async Task<IActionResult> FScanLogsAsync(string? key, int currentPage, int pageSize)
    {
        var result = await _reports.FScanLogsAsync(key, currentPage, pageSize);
        return Ok(result);

    }


    [HttpPost("FScanLogsDetails")]
    public async Task<IActionResult> FScanLogsDetailsAsync([FromBody] FSLogDetailsRequest request)
    {
        var result = await _reports.FScanLogsDetailsAsync(request);
        return Ok(result);

    }

    [HttpPost("ExportToPDF")]
    public async Task<IActionResult> ExportToPDF([FromBody] PdfRequest sheet)
    {
        var result = await _reports.ExportToPDF(sheet);
        return Ok(result);

    }

}
