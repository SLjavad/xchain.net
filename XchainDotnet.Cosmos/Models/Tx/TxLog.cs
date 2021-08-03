using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace XchainDotnet.Cosmos.Models.Tx
{
    public class TxLog
    {
        [JsonPropertyName("msg_index")]
        public int MsgIndex { get; set; }
        [JsonPropertyName("log")]
        public string Log { get; set; }
        [JsonPropertyName("events")]
        public List<TxEvent> Events { get; set; }
    }
}
