using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using XchainDotnet.Client.Models;

namespace XchainDotnet.Thorchain.Models
{
    public class TxData
    {
        /// <summary>
        /// Transaction Type
        /// </summary>
        [JsonPropertyName("type")]
        public TxType Type { get; set; }
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
    }
}
