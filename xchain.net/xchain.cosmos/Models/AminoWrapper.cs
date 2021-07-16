using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Xchain.net.xchain.cosmos.Models
{
    public class AminoWrapper
    {
        public AminoWrapper(string type, object value)
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
        public object Value { get; set; }

    }
}
