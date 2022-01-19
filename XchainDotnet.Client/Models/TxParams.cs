using System.Text.Json.Serialization;

namespace XchainDotnet.Client.Models
{
    public class TxParams
    {
        [JsonPropertyName("walletIndex")]
        public int WalletIndex { get; set; } = 0;
        [JsonPropertyName("asset")]
        public Asset Asset { get; set; }
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        [JsonPropertyName("recipient")]
        public string Recipient { get; set; }
        [JsonPropertyName("memo")]
        public string Memo { get; set; }
    }
}
