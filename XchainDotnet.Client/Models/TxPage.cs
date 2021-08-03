using System.Collections.Generic;

namespace XchainDotnet.Client.Models
{
    public class TxPage
    {
        public int Total { get; set; }
        public List<Tx> Txs { get; set; }
    }
}
