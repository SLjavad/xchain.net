using System.Text.Json.Serialization;

namespace XchainDotnet.Cosmos.Models.Tx
{
    public class BroadcastTxParams
    {
        [JsonPropertyName("tx")]
        public StdTx Tx { get; set; }
        [JsonPropertyName("mode")]
        public string Mode { get; set; }
    }
}
