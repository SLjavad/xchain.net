using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xchain.net.xchain.cosmos.Models.Message.Base;

namespace Xchain.net.xchain.cosmos.Models
{
    public class AminoWrapper<T> : Msg
    {
        public AminoWrapper(string type, T value)
        {
            Type = type;
            Value = value;
        }

        public AminoWrapper()
        {

        }

        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("value")]
        public T Value { get; set; }

    }
}
