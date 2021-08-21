using System.Collections.Generic;
using System.Text.Json.Serialization;
using XchainDotnet.Cosmos.Models.Address;
using XchainDotnet.Cosmos.Models.Message.Base;

namespace XchainDotnet.Thorchain.Models.Message
{
    public class MsgNativeTx : Msg
    {
        /// <summary>
        /// MsgNativeTx
        /// </summary>
        /// <param name="coins">Message coins</param>
        /// <param name="memo">memo</param>
        /// <param name="signer">signer address object</param>
        public MsgNativeTx(List<MsgCoin> coins, string memo, AccAddress signer)
        {
            Coins = coins;
            Memo = memo;
            Signer = signer;
        }

        public MsgNativeTx()
        {
            
        }

        /// <summary>
        /// Message coins
        /// </summary>
        [JsonPropertyName("coins")]
        public List<MsgCoin> Coins { get; set; }
        /// <summary>
        /// memo
        /// </summary>
        [JsonPropertyName("memo")]
        public string Memo { get; set; }
        /// <summary>
        /// signer address object
        /// </summary>
        [JsonPropertyName("signer")]
        public AccAddress Signer { get; set; }


        /// <summary>
        /// create new MsgNativeTx
        /// </summary>
        /// <param name="coins">Message coins</param>
        /// <param name="memo">memo</param>
        /// <param name="signer">signer address</param>
        /// <returns></returns>
        public static MsgNativeTx MsgNativeFromJson(List<MsgCoin> coins, string memo, string signer)
        {
            return new MsgNativeTx(coins, memo, AccAddress.FromBech32(signer));
        }

    }
}
