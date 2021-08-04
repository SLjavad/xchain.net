using System.Text.Json.Serialization;

namespace XchainDotnet.Thorchain.Models.Message
{
    public class MsgCoin
    {
        [JsonPropertyName("asset")]
        public string Asset { get; set; }
        [JsonPropertyName("amount")]
        public string Amount { get; set; }
    }
}
