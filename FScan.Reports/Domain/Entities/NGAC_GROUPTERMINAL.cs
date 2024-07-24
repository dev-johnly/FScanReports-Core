using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Domain.Entities
{
    public class NGAC_GROUPTERMINAL
    {
        [Key]
        public int GroupID { get; set; }
        public int TerminalID { get; set; }
        public int Saved { get; set; }

        public NGAC_GROUP NGAC_GROUP { get; set; }
    }
}
