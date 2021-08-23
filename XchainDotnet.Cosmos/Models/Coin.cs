using System.Text.Json.Serialization;

namespace XchainDotnet.Cosmos.Models
{
    /// <summary>
    /// Coin object
    /// </summary>
    public class Coin
    {
        /// <summary>
        /// Coin Denomination
        /// </summary>
        [JsonPropertyName("denom")]
        public string Denom { get; set; }
        /// <summary>
        /// Coin amount
        /// </summary>
        [JsonPropertyName("amount")]
        public string Amount { get; set; }
    }
}
