using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Xchain.net.xchain.cosmos.Models.Tx
{
    public class StdTxFee
    {
        [JsonPropertyName("gas")]
        public string Gas { get; set; }
        [JsonPropertyName("amount")]
        public List<Coin> Amount { get; set; }
    }
}
