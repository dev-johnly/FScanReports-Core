using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Domain.Entities
{
    public class NGAC_GROUP
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long? Code { get; set; }
        public int? tzCode { get; set; }
    }
}
