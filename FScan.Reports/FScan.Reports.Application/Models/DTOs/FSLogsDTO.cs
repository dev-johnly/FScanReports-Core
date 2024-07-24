using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FScan.Reports.Application.Models.DTOs
{
    public class FSLogsDTO
    {
        [JsonPropertyName("indexKey")]
        public long IndexKey { get; set; }

        [JsonPropertyName("id")]
        public string ID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("groupName")]
        public string GroupName { get; set; }

        [JsonPropertyName("accessType")]
        public string AccessType { get; set; }
    }
}
