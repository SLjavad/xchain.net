using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace XchainDotnet.Client.Models
{
    public class TxFrom
    {
        /// <summary>
        /// From Address
        /// </summary>
        [JsonPropertyName("from")]
        public string From { get; set; }
        /// <summary>
        /// transaction amount
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
    }

    public class TxTo
    {
        /// <summary>
        /// To Address
        /// </summary>
        [JsonPropertyName("to")]
        public string To { get; set; }
        /// <summary>
        /// transaction amount
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
    }

    public enum TxType
    {
        transfer,
        unknown
    }

    /// <summary>
    /// object representing transaction
    /// </summary>
    public class Tx
    {
        /// <summary>
        /// transaction asset
        /// </summary>
        [JsonPropertyName("asset")]
        public Asset Asset { get; set; }
        /// <summary>
        /// List of from addresses
        /// </summary>
        [JsonPropertyName("from")]
        public List<TxFrom> From { get; set; }
        /// <summary>
        /// List of to addresses
        /// </summary>
        [JsonPropertyName("to")]
        public List<TxTo> To { get; set; }
        /// <summary>
        /// transaction datetime
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        /// <summary>
        /// Transaction Type
        /// </summary>
        [JsonPropertyName("type")]
        public TxType Type { get; set; }
        /// <summary>
        /// Trasnsaction Hash
        /// </summary>
        [JsonPropertyName("hash")]
        public string Hash { get; set; }
    }
}
