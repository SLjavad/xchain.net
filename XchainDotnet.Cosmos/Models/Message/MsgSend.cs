using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using XchainDotnet.Cosmos.Models.Address;
using XchainDotnet.Cosmos.Models.Message.Base;

namespace XchainDotnet.Cosmos.Models.Message
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
