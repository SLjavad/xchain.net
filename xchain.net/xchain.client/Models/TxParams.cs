using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchain.net.xchain.thorchain.Models;

namespace Xchain.net.xchain.client.Models
{
    public class TxParams
    {
        public Asset Asset { get; set; } = new AssetRune();
        public decimal Amount { get; set; }
        public string Recipient { get; set; }
        public string Memo { get; set; }
    }
}
