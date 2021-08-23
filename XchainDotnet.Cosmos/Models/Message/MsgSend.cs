using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using XchainDotnet.Cosmos.Models.Address;
using XchainDotnet.Cosmos.Models.Message.Base;

namespace XchainDotnet.Cosmos.Models.Message
{
    /// <summary>
    /// Send message
    /// </summary>
    public class MsgSend : Msg
    {
        /// <summary>
        /// Send message
        /// </summary>
        /// <param name="fromAddress">the address which is sending from</param>
        /// <param name="toAddress">the address which is sending to</param>
        /// <param name="amount">amounts to send</param>
        public MsgSend(AccAddress fromAddress, AccAddress toAddress, List<Coin> amount)
        {
            FromAddress = fromAddress;
            ToAddress = toAddress;
            Amount = amount;
        }

        public MsgSend()
        {

        }

        /// <summary>
        /// the address which is sending from
        /// </summary>
        [JsonPropertyName("from_address")]
        public AccAddress FromAddress { get; set; }
        /// <summary>
        /// the address which is sending to
        /// </summary>
        [JsonPropertyName("to_address")]
        public AccAddress ToAddress { get; set; }
        /// <summary>
        /// amount to send
        /// </summary>
        [JsonPropertyName("amount")]
        public List<Coin> Amount { get; set; }

        public static MsgSend FromJson(string jsonValue)
        {
            var msgSend = JsonSerializer.Deserialize<MsgSend>(jsonValue);
            return msgSend;
        }
    }
}
