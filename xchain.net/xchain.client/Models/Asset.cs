using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Xchain.net.xchain.client.Models
{
    public record Asset
    {
        [JsonPropertyName("chain")]
        public virtual Chain Chain { get; set; }
        [JsonPropertyName("symbol")]
        public virtual string Symbol { get; set; }
        [JsonPropertyName("ticker")]
        public virtual string Ticker { get; set; }
    }
}
