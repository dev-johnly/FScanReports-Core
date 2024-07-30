using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Application.Models.Requests
{
    public class PagedResultRequest
    {
        public string? key { get; set; }
        public int currentPage { get; set; } = 1;
        public int pageSize { get; set; } = 10;

    }
}
