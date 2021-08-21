using System.Text.Json.Serialization;

namespace XchainDotnet.Thorchain.Models.Message
{
    public class MsgCoin
    {
        /// <summary>
        /// Asset name
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; }
        /// <summary>
        /// Amount of coin
        /// </summary>
        [JsonPropertyName("amount")]
        public string Amount { get; set; }
    }
}
