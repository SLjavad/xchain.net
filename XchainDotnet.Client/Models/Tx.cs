using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace XchainDotnet.Client.Models
{
    public class TxFrom
    {
        [JsonPropertyName("from")]
        public string From { get; set; }
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
    }

    public class TxTo
    {
        [JsonPropertyName("to")]
        public string To { get; set; }
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
    }

    public enum TxType
    {
        transfer,
        unknown
    }

    public class Tx
    {
        [JsonPropertyName("asset")]
        public Asset Asset { get; set; }
        [JsonPropertyName("from")]
        public List<TxFrom> From { get; set; }
        [JsonPropertyName("to")]
        public List<TxTo> To { get; set; }
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [JsonPropertyName("type")]
        public TxType Type { get; set; }
        [JsonPropertyName("hash")]
        public string Hash { get; set; }
    }
}
