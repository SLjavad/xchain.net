using System;
using XchainDotnet.Client.Models;
using XchainDotnet.Cosmos.Models.RPC;

namespace XchainDotnet.Thorchain.Models
{
    /// <summary>
    /// Transaction search options
    /// </summary>
    public class TxHistoryParamFilter : TxHistoryParams
    {
        /// <summary>
        /// Custom function for filter result
        /// </summary>
        public Func<RPCTxResult, bool> FilterFn { get; set; }
    }
}
