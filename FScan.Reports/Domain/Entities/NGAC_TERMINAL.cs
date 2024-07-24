using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Domain.Entities
{
    public class NGAC_TERMINAL
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MacAddress { get; set; }
        public DateTime regDate { get; set; }
        public int APB1 { get; set; }
        public int APB2 { get; set; }
        public int HardwareID { get; set; }
        public int TimeZoneCode { get; set; }
        public int Reserved { get; set; }
        public int UTCIndex { get; set; }
        public int OptFlag { get; set; }
        public string IPAddress { get; set; }
        public int MasterID { get; set; }
    }
}
