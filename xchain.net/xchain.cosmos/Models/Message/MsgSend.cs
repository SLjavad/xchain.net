using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xchain.net.xchain.cosmos.Models.Address;
using Xchain.net.xchain.cosmos.Models.Message.Base;

namespace Xchain.net.xchain.cosmos.Models.Message
{
    public class MsgSend : Msg
    {
        public MsgSend(AccAddress fromAddress, AccAddress toAddress, List<Coin> amount)
        {
            FromAddress = fromAddress;
            ToAddress = toAddress;
            Amount = amount;
        }

        public MsgSend()
        {

        }

        [JsonPropertyName("from_address")]
        public AccAddress FromAddress { get; set; }
        [JsonPropertyName("to_address")]
        public AccAddress ToAddress { get; set; }
        [JsonPropertyName("amount")]
        public List<Coin> Amount { get; set; }

        public static MsgSend FromJson(string jsonValue)
        {
            var msgSend = JsonSerializer.Deserialize<MsgSend>(jsonValue);
            return msgSend;
        }
    }
}
