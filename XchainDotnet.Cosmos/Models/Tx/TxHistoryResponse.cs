using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace XchainDotnet.Cosmos.Models.Tx
{
    public class TxHistoryResponse
    {
        [JsonPropertyName("total_count")]
        public int? TotalCount { get; set; }
        [JsonPropertyName("count")]
        public int? Count { get; set; }
        [JsonPropertyName("page_number")]
        public int? PageNumber { get; set; }
        [JsonPropertyName("page_total")]
        public int? PageTotal { get; set; }
        [JsonPropertyName("limit")]
        public int? Limit { get; set; }
        [JsonPropertyName("txs")]
        public List<TxResponse> Txs { get; set; }
    }
}
