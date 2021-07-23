using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xchain.net.xchain.cosmos.Models.Address;
using Xchain.net.xchain.cosmos.Models.Message.Base;

namespace Xchain.net.xchain.cosmos.Models.Message
{
    public class MsgDeposit : Msg
    {
        public MsgDeposit(List<MsgCoinDeposit> coins, string memo, AccAddress signer)
        {
            Coins = coins;
            Memo = memo;
            Signer = signer;
        }

        public MsgDeposit()
        {

        }

        [JsonPropertyName("coins")]
        public List<MsgCoinDeposit> Coins { get; set; }
        [JsonPropertyName("memo")]
        public string Memo { get; set; }
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
