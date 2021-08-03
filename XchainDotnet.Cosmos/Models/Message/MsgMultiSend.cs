﻿using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using XchainDotnet.Cosmos.Models.Message.Base;

namespace XchainDotnet.Cosmos.Models.Message
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

    public class MsgMultiSend : Msg
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
