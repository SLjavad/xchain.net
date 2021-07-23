using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Xchain.net.xchain.cosmos.Models
{
    public class Coin
    {
        [JsonPropertyName("denom")]
        public string Denom { get; set; }
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
    }
}
