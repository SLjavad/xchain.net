using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace XchainDotnet.Cosmos.Models.Tx
{
    public class StdTxFee
    {
        [JsonPropertyName("gas")]
        public string Gas { get; set; }
        [JsonPropertyName("amount")]
        public List<Coin> Amount { get; set; }
    }
}
