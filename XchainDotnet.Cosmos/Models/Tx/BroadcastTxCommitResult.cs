using System.Text.Json.Serialization;

namespace XchainDotnet.Cosmos.Models.Tx
{
    /// <summary>
    /// Broadcast tx Result
    /// </summary>
    public class BroadcastTxCommitResult
    {
        [JsonPropertyName("data")]
        public string Data { get; set; }
        [JsonPropertyName("raw_log")]
        public string RawLog { get; set; }
        [JsonPropertyName("logs")]
        public object Logs { get; set; } //TODO: should make a model for this if needed later
        [JsonPropertyName("gas_wanted")]
        public string GasWanted { get; set; }
        [JsonPropertyName("gas_used")]
        public string GasUsed { get; set; }
        [JsonPropertyName("txhash")]
        public string TxHash { get; set; }
        [JsonPropertyName("height")]
        public string Height { get; set; }

        [JsonPropertyName("check_tx")]
        public CheckTxResult CheckTxResult { get; set; }
        [JsonPropertyName("deliver_tx")]
        public DeliverTxResult DeliverTxResult { get; set; }
    }
}
