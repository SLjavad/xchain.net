using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Xchain.net.xchain.cosmos.Models.Tx
{
    public class BroadcastTxParams
    {
        [JsonPropertyName("tx")]
        public StdTx Tx { get; set; }
        [JsonPropertyName("mode")]
        public string Mode { get; set; }
    }
}
