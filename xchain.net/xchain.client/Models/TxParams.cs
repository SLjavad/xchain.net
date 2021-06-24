using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchain.net.xchain.client.Models
{
    public class TxParams
    {
        public Asset Asset { get; set; }
        public decimal Amount { get; set; }
        public string Recipient { get; set; }
        public string Memo { get; set; }
    }
}
