using FScan.Reports.Application.Contracts;
using FScan.Reports.Application.Models.DTOs;
using FScan.Reports.Application.Models.Requests;
using FScan.Reports.Application.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FScanReports.Server.Controller;

[Route("api/[controller]")]
[ApiController]
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
}
