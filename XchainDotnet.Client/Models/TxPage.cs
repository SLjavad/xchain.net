using System.Collections.Generic;

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
        public int Total { get; set; }
        /// <summary>
        /// Transaction list of current page
        /// </summary>
        public List<Tx> Txs { get; set; }
    }
}
