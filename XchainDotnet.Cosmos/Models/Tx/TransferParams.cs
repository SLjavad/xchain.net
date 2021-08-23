using System.Text.Json.Serialization;
using XchainDotnet.Cosmos.Models.Crypto;

namespace XchainDotnet.Cosmos.Models.Tx
{
    /// <summary>
    /// Transfer Options
    /// </summary>
    public class TransferParams
    {
        /// <summary>
        /// Private key options
        /// </summary>
        [JsonPropertyName("privkey")]
        public IPrivateKey PrivKey { get; set; }
        /// <summary>
        /// the address which is transfering from
        /// </summary>
        [JsonPropertyName("from")]
        public string From { get; set; }
        /// <summary>
        /// the address which is transfering to
        /// </summary>
        [JsonPropertyName("to")]
        public string To { get; set; }
        /// <summary>
        /// Trasnfer amount
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        /// <summary>
        /// Transfer assets
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; }
        /// <summary>
        /// transfer memo
        /// </summary>
        [JsonPropertyName("memo")]
        public string Memo { get; set; }
        /// <summary>
        /// Transfer fee
        /// </summary>
        [JsonPropertyName("fee")]
        public StdTxFee Fee { get; set; }
    }
}
