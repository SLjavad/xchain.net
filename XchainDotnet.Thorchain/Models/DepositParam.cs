using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xchain.net.xchain.client.Models;

namespace Xchain.net.xchain.thorchain.Models
{
    public class DepositParam
    {
        [JsonPropertyName("asset")]
        public Asset Asset { get; set; } = new AssetRune();
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        [JsonPropertyName("memo")]
        public string Memo { get; set; }
    }
}
