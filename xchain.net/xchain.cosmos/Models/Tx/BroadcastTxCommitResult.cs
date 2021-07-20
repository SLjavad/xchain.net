using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Xchain.net.xchain.cosmos.Models.Tx
{
    public class BroadcastTxCommitResult
    {
        [JsonPropertyName("check_tx")]
        public CheckTxResult CheckTx { get; set; }
        [JsonPropertyName("deliver_tx")]
        public DeliverTxResult DeliverTx { get; set; }
        [JsonPropertyName("txhash")]
        public string TxHash { get; set; }
        [JsonPropertyName("height")]
        public int? Height { get; set; }
    }
}
