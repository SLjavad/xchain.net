using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchain.net.xchain.client.Models;
using Xchain.net.xchain.cosmos.Models.RPC;

namespace Xchain.net.xchain.thorchain.Models
{
    public class TxHistoryParamFilter : TxHistoryParams
    {
        public Func<RPCTxResult, bool> FilterFn { get; set; }
    }
}
