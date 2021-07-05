using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchain.net.xchain.client.Models
{
    public class TxPage
    {
        public int Total { get; set; }
        public List<Tx> Txs { get; set; }
    }
}
