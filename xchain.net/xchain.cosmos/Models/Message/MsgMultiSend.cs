using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xchain.net.xchain.cosmos.Models.Message.Base;

namespace Xchain.net.xchain.cosmos.Models.Message
{

    public class Input
    {
        [JsonPropertyName("address")]
        public string Address { get; set; }
        [JsonPropertyName("coins")]
        public List<Coin> Coins { get; set; }
    }

    public class Output
    {
        [JsonPropertyName("address")]
        public string Address { get; set; }
        [JsonPropertyName("coins")]
        public List<Coin> Coins { get; set; }
    }

    public class MsgMultiSend : IMsg
    {
        public MsgMultiSend(List<Input> inputs, List<Output> outputs)
        {
            Inputs = inputs;
            Outputs = outputs;
        }

        public MsgMultiSend()
        {

        }

        [JsonPropertyName("inputs")]
        public List<Input> Inputs { get; set; }
        [JsonPropertyName("outputs")]
        public List<Output> Outputs { get; set; }


        public static MsgMultiSend FromJson(string jsonValue)
        {
            var msgMultiSend = JsonSerializer.Deserialize<MsgMultiSend>(jsonValue);
            return msgMultiSend;
        }

    }
}
