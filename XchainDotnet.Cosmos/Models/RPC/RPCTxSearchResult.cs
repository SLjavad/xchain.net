using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace XchainDotnet.Cosmos.Models.RPC
{
    /// <summary>
    /// result object for RPC tx search
    /// </summary>
    public class RPCTxSearchResult
    {
        /// <summary>
        /// Transactions
        /// </summary>
        [JsonPropertyName("txs")]
        public List<RPCTxResult> Txs { get; set; }
        /// <summary>
        /// Total Transactions Count
        /// </summary>
        [JsonPropertyName("total_count")]
        public string TotalCount { get; set; }
    }
}
