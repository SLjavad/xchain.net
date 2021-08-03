using System.Text.Json.Serialization;

namespace XchainDotnet.Cosmos.Models
{
    public class Coin
    {
        [JsonPropertyName("denom")]
        public string Denom { get; set; }
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
    }
}
