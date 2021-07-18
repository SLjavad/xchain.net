using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Xchain.net.xchain.cosmos.Models.RPC
{
    public class RPCTxSearchResult
    {
        [JsonPropertyName("txs")]
        public List<RPCTxResult> Txs { get; set; }
        [JsonPropertyName("total_count")]
        public string TotalCount { get; set; }
    }
}
