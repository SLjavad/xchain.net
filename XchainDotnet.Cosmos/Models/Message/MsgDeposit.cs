using System.Collections.Generic;
using System.Text.Json.Serialization;
using XchainDotnet.Cosmos.Models.Address;
using XchainDotnet.Cosmos.Models.Message.Base;

namespace XchainDotnet.Cosmos.Models.Message
{
    /// <summary>
    /// Deposit message
    /// </summary>
    public class MsgDeposit : Msg
    {
        /// <summary>
        /// Deposit Message
        /// </summary>
        /// <param name="coins">message coins</param>
        /// <param name="memo">message memo</param>
        /// <param name="signer">message signer address object</param>
        public MsgDeposit(List<MsgCoinDeposit> coins, string memo, AccAddress signer)
        {
            Coins = coins;
            Memo = memo;
            Signer = signer;
        }

        public MsgDeposit()
        {

        }

        /// <summary>
        /// message coins
        /// </summary>
        [JsonPropertyName("coins")]
        public List<MsgCoinDeposit> Coins { get; set; }
        /// <summary>
        /// Message memo
        /// </summary>
        [JsonPropertyName("memo")]
        public string Memo { get; set; }
        /// <summary>
        /// Message signer address object
        /// </summary>
        [JsonPropertyName("signer")]
        public AccAddress Signer { get; set; }
    }

    public class MsgCoinDeposit
    {
        [JsonPropertyName("asset")]
        public string Asset { get; set; }
        [JsonPropertyName("amount")]
        public string Amount { get; set; }
    }
}
