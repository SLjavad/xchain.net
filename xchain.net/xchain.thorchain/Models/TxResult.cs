using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Xchain.net.xchain.thorchain.Models
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
        public List<ObservedTx_Coin> Coins { get; set; }
        [JsonPropertyName("gas")]
        public List<ObservedTx_Gas> Gas { get; set; }
        [JsonPropertyName("memo")]
        public string Memo { get; set; }
    }

    public class ObservedTx_Coin
    {
        [JsonPropertyName("asset")]
        public string Asset { get; set; }
        [JsonPropertyName("amount")]
        public string Amount { get; set; }
    }

    public class ObservedTx_Gas
    {
        [JsonPropertyName("asset")]
        public string Asset { get; set; }
        [JsonPropertyName("amount")]
        public string Amount { get; set; }
    }

    public class ObservedTx
    {
        [JsonPropertyName("observer_tx")]
        public ObservedTx_Tx Tx { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("signers")]
        public string Signers { get; set; }
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
