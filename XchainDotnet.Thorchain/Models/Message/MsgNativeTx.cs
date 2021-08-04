using System.Collections.Generic;
using System.Text.Json.Serialization;
using XchainDotnet.Cosmos.Models.Address;
using XchainDotnet.Cosmos.Models.Message.Base;

namespace XchainDotnet.Thorchain.Models.Message
{
    public class MsgNativeTx : Msg
    {
        public MsgNativeTx(List<MsgCoin> coins, string memo, AccAddress signer)
        {
            Coins = coins;
            Memo = memo;
            Signer = signer;
        }

        public MsgNativeTx()
        {

        }

        [JsonPropertyName("coins")]
        public List<MsgCoin> Coins { get; set; }
        [JsonPropertyName("memo")]
        public string Memo { get; set; }
        [JsonPropertyName("signer")]
        public AccAddress Signer { get; set; }


        public static MsgNativeTx MsgNativeFromJson(List<MsgCoin> coins, string memo, string signer)
        {
            return new MsgNativeTx(coins, memo, AccAddress.FromBech32(signer));
        }

    }
}
