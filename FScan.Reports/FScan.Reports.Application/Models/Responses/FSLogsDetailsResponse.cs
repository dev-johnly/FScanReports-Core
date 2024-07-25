using FScan.Reports.Application.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Application.Models.Responses;

public class FSLogsDetailsResponse
{
    public string? DetailsName { get; set; }

    public string? DateFrom { get; set; }
    public string? DateTo { get; set; }

    public string? DetailsId { get; set; }

    public string? GroupName { get; set; }


    public string? GetLogs { get; set; }

    public List<FSLogsDetailsDTO> GetLogsList;
}
