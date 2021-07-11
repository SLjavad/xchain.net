using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Xchain.net.xchain.cosmos.Models
{
    public class MsgCoin
    {
        [JsonPropertyName("asset")]
        public string Asset { get; set; }
        [JsonPropertyName("amount")]
        public string Amount { get; set; }
    }
}
