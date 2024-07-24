using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FScan.Reports.Application.Models.DTOs
{
    public class AttendanceDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("attendanceDate")]
        public DateTime? AttendanceDate { get; set; }

        [JsonPropertyName("timeIn")]
        public DateTime? TimeIn { get; set; }

        [JsonPropertyName("timeOut")]
        public DateTime? TimeOut { get; set; }

        [JsonPropertyName("lunchOut")]
        public DateTime? LunchOut { get; set; }

        [JsonPropertyName("lunchIn")]
        public DateTime? LunchIn { get; set; }

        [JsonPropertyName("totalTime")]
        public TimeSpan? TotalTime { get; set; }

        [JsonPropertyName("totalLunchTime")]
        public TimeSpan? TotalLunchTime { get; set; }

        [JsonPropertyName("timeInText")]
        public string? TimeInText { get; set; }

        [JsonPropertyName("timeOutText")]
        public string? TimeOutText { get; set; }

        [JsonPropertyName("lunchInText")]
        public string? LunchInText { get; set; }

        [JsonPropertyName("lunchOutText")]
        public string? LunchOutText { get; set; }

        [JsonPropertyName("attendanceDateText")]
        public string? AttendanceDateText { get; set; }

    }
}
