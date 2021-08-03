using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace XchainDotnet.Cosmos.Models.RPC
{
    public class RPCTxSearchResult
    {
        [JsonPropertyName("txs")]
        public List<RPCTxResult> Txs { get; set; }
        [JsonPropertyName("total_count")]
        public string TotalCount { get; set; }
    }
}
