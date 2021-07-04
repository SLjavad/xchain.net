using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Xchain.net.xchain.cosmos.Models.Tx
{
    public class TxEvent
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("attributes")]
        public List<TxEventAttribute> Attributes { get; set; }
    }
}
