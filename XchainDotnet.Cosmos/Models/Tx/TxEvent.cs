using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace XchainDotnet.Cosmos.Models.Tx
{
    public class TxEvent
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("attributes")]
        public List<TxEventAttribute> Attributes { get; set; }
    }
}
