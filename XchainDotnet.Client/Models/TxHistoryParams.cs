using System;
using System.Text.Json.Serialization;

namespace XchainDotnet.Client.Models
{
    public class TxHistoryParams
    {
        [JsonPropertyName("address")]
        public string Address { get; set; }
        [JsonPropertyName("offset")]
        public int Offset { get; set; }
        [JsonPropertyName("limit")]
        public int Limit { get; set; }
        [JsonPropertyName("startTime")]
        public DateTime StartTime { get; set; }
        [JsonPropertyName("asset")]
        public string Asset { get; set; }
    }
}
