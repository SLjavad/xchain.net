using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Xchain.net.xchain.cosmos.Models.Tx
{
    public class DeliverTxResult
    {
        [JsonPropertyName("code")]
        public int? Code { get; set; }
        [JsonPropertyName("data")]
        public string Data { get; set; }
        [JsonPropertyName("gas_used")]
        public int? GasUsed { get; set; }
        [JsonPropertyName("gas_wanted")]
        public int? GasWanted { get; set; }
        [JsonPropertyName("info")]
        public string Info { get; set; }
        [JsonPropertyName("log")]
        public string Log { get; set; }
        [JsonPropertyName("tags")]
        public List<KeyValuePair<string , string>> Tags { get; set; }
    }
}
