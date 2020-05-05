
using System;
using Newtonsoft.Json;

namespace PresidentsApi.Models
{
    public class PresidentInfo
    {
        public int Number { get; set; }

        [JsonProperty("president")]
        public string Name { get; set; }

        [JsonProperty("death_year")]
        public int? DeathYear { get; set; }

        [JsonProperty("took_office")]
        public DateTime TermStart { get; set; }

        [JsonProperty("left_office")]
        public DateTime? TermEnd { get; set; }

        public string Party { get; set; }
    }
}