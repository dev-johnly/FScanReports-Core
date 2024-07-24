using FScan.Reports.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Domain.Entities
{
    public class NGAC_USERINFO 
    {
        [Key]
        public long IndexKey { get; set; }
        public string ID { get; set; }
        public string? Name { get; set; }
        public int? GroupID { get; set; }
        public string? EmployeeNum { get; set; }
        public string? Department { get; set; }
        public string? Description { get; set; }
        public int? Privilege { get; set; }
        public int? AuthType { get; set; }
        public DateTime? regDate { get; set; }
        public DateTime? expDate { get; set; }
        public string? Password { get; set; }
        public long? RFID { get; set; }
        public int? tzCode { get; set; }
        public  int? statusAPB { get; set; }
        public int? TemplateCnt { get; set; }
        public DateTime? SvrRecordTime { get; set; }
        public  int? Position { get; set; }
        public string? EMail { get; set; }
        public int? LastPWTime { get; set; }
        public int? OptionVal { get; set; }
        public  int? FaceCount { get; set; }
        public string? PhoneNo { get; set; }
        public int? IssueCount { get; set; }
        public int? StoreCode { get; set; }
        public int? VendorCode { get; set; }
        public int? VoIPuse { get; set; }
        public int?  DoorOpen1 { get; set; }
        public  int? DoorOpen2 { get; set; }
        public string? Number { get; set; }
        public string? FSPassword { get; set; }
        public string? AccessType { get; set; }

        public string? BankcomEmail { get; set; }
        public bool MustChangePW { get; set; }

    }
}
