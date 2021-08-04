using System.Collections.Generic;
using System.Text.Json.Serialization;
using XchainDotnet.Thorchain.Models.Message;

namespace XchainDotnet.Thorchain.Models
{

    public class ObservedTx_Tx
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("chain")]
        public string Chain { get; set; }
        [JsonPropertyName("from_address")]
        public string FromAddress { get; set; }
        [JsonPropertyName("to_address")]
        public string ToAddress { get; set; }
        [JsonPropertyName("coins")]
        public List<MsgCoin> Coins { get; set; }
        [JsonPropertyName("gas")]
        public List<MsgCoin> Gas { get; set; }
        [JsonPropertyName("memo")]
        public string Memo { get; set; }
    }

    public class ObservedTx
    {
        [JsonPropertyName("tx")]
        public ObservedTx_Tx Tx { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("signers")]
        public List<string> Signers { get; set; }
    }

    public class KeysignMetric
    {
        [JsonPropertyName("tx_id")]
        public string TxId { get; set; }
        [JsonPropertyName("node_tss_times")]
        public object NodeTssTimes { get; set; }
    }

    public class TxResult
    {
        [JsonPropertyName("observed_tx")]
        public ObservedTx ObservedTx { get; set; }
        [JsonPropertyName("keysign_metric")]
        public KeysignMetric KeysignMetric { get; set; }
    }
}
