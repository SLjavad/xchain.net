using System;
using XchainDotnet.Client.Models;
using XchainDotnet.Cosmos.Models.RPC;

namespace XchainDotnet.Thorchain.Models
{
    public class TxHistoryParamFilter : TxHistoryParams
    {
        public Func<RPCTxResult, bool> FilterFn { get; set; }
    }
}
