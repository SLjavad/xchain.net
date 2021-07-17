using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xchain.net.xchain.cosmos.Models.Tx;

namespace Xchain.net.xchain.cosmos.Models.RPC
{

    public class TxResult_RPC
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("data")]
        public string Data { get; set; }
        [JsonPropertyName("log")]
        public string Log { get; set; }
        [JsonPropertyName("info")]
        public string Info { get; set; }
        [JsonPropertyName("gas_wanted")]
        public string GasWanted { get; set; }
        [JsonPropertyName("gas_used")]
        public string GasUsed { get; set; }
        [JsonPropertyName("events")]
        public List<TxEvent> Events { get; set; }
        [JsonPropertyName("codespace")]
        public string CodeSpace { get; set; }
    }

    public class RPCTxResult
    {
        [JsonPropertyName("hash")]
        public string Hash { get; set; }
        [JsonPropertyName("height")]
        public string Height { get; set; }
        [JsonPropertyName("number")]
        public int Number { get; set; }
        [JsonPropertyName("tx")]
        public string Tx { get; set; }
        [JsonPropertyName("tx_result")]
        public TxResult_RPC TxResult { get; set; }
    }
}
