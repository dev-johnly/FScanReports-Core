using FScan.Reports.Application.Models.DTOs;
using FScan.Reports.Application.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Application.Contracts;

public interface IReportsRepository
{
    Task<TimeSheetDTO> TimeSheetsAsync();

    Task<TimeSheetDTO> GetFilteredTimeSheetAsync(DateFilterRequest request);

}
