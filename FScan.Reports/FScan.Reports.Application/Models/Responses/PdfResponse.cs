using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Application.Models.Responses
{
    public class PdfResponse
    {
        public string Filename { get; set; }

        public string ErrorMessage { get; set; }


        public byte[] FileBytes { get; set; }
    }
}
