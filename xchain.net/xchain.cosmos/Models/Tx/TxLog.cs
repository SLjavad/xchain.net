using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Xchain.net.xchain.cosmos.Models.Tx
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
