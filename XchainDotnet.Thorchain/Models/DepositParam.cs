using System.Text.Json.Serialization;
using XchainDotnet.Client.Models;

namespace XchainDotnet.Thorchain.Models
{
    /// <summary>
    /// Options for deposit
    /// </summary>
    public class DepositParam
    {
        /// <summary>
        /// Deposit asset
        /// </summary>
        [JsonPropertyName("asset")]
        public Asset Asset { get; set; } = new AssetRune();
        /// <summary>
        /// Deposit Amount
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        /// <summary>
        /// Deposit Memo
        /// </summary>
        [JsonPropertyName("memo")]
        public string Memo { get; set; }
        /// <summary>
        /// Deposit Wallet Index
        /// </summary>
        [JsonPropertyName("walletIndex")]
        public int? WalletIndex { get; set; }
    }
}
