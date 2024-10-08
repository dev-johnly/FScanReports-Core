﻿using FScan.Reports.Application.Models.DTOs;
using FScan.Reports.Application.Models.Helpers;
using FScan.Reports.Application.Models.Requests;
using FScan.Reports.Application.Models.Responses;
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

    Task<PagedResult<FSLogsDTO>> FScanLogsAsync(string? key, int currentPage, int pageSize);

    Task<FSLogsDetailsResponse> FScanLogsDetailsAsync(FSLogDetailsRequest request);

    Task<PdfResponse> ExportToPDF(PdfRequest sheet);
}
