using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xchain.net.xchain.cosmos.Models.Address;
using Xchain.net.xchain.cosmos.Models.Message.Base;

namespace Xchain.net.xchain.cosmos.Models
{
    public class MsgNativeTx : IMsg
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


    }
}
