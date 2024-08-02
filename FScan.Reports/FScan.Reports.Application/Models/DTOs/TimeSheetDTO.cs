using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FScan.Reports.Application.Models.DTOs
{
    public class TimeSheetDTO
    {
        [JsonPropertyName("empId")]
        public string EmpId { get; set; }

        [JsonPropertyName("empName")]
        public string EmpName { get; set; }

        [JsonPropertyName("unit")]
        public string Unit { get; set; }

        [JsonPropertyName("dateFrom")]
        public DateTime? DateFrom { get; set; }

        [JsonPropertyName("dateTo")]
        public DateTime? DateTo { get; set; }

        [JsonPropertyName("attendanceList")]
        public List<AttendanceDTO> AttendanceList { get; set; }

        [JsonPropertyName("logsDetails")]
        public List<FSLogsDTO> LogsDetails { get; set; }
    }
}
