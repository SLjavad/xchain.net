using System.Text.Json.Serialization;
using XchainDotnet.Client.Models;

namespace XchainDotnet.Thorchain.Models
{
    public class DepositParam
    {
        [JsonPropertyName("asset")]
        public Asset Asset { get; set; } = new AssetRune();
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        [JsonPropertyName("memo")]
        public string Memo { get; set; }
    }
}
