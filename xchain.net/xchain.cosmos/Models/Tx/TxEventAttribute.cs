using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Xchain.net.xchain.cosmos.Models.Tx
{
    public class TxEventAttribute
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
