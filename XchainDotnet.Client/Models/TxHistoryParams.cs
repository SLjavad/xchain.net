using System;
using System.Text.Json.Serialization;

namespace XchainDotnet.Client.Models
{
    public class TxHistoryParams
    {
        /// <summary>
        /// address
        /// </summary>
        [JsonPropertyName("address")]
        public string Address { get; set; }
        /// <summary>
        /// offset
        /// </summary>
        [JsonPropertyName("offset")]
        public int? Offset { get; set; }
        /// <summary>
        /// Result Limit
        /// </summary>
        [JsonPropertyName("limit")]
        public int? Limit { get; set; }
        [JsonPropertyName("startTime")]
        public DateTime StartTime { get; set; }
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; }
    }
}
