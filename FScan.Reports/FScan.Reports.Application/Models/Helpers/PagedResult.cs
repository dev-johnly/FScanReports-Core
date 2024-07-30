using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Application.Models.Helpers
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = [];
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }
}
