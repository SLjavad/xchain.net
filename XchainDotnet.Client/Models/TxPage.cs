using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace XchainDotnet.Client.Models
{
    /// <summary>
    /// an object representing tx list in page
    /// </summary>
    public class TxPage
    {
        /// <summary>
        /// Total amount of result
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; set; }
        /// <summary>
        /// Transaction list of current page
        /// </summary>
        [JsonPropertyName("txs")]
        public List<Tx> Txs { get; set; }
    }
}
