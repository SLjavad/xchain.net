using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Xchain.net.xchain.client.Models
{
    public enum FeeOptionKey
    {
        average,
        fast,
        fastest
    }

    public enum FeeType
    {
        @byte,
        @base
    }

    public class Fees
    {
        [JsonPropertyName("fast")]
        public decimal Fast { get; set; }
        [JsonPropertyName("fastest")]
        public decimal Fastest { get; set; }
        [JsonPropertyName("average")]
        public decimal Average { get; set; }
        [JsonPropertyName("type")]
        public FeeType Type { get; set; }
    }

    public class FeeParams
    {

    }
}
