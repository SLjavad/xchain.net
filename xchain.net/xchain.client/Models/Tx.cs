using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchain.net.xchain.client.Models
{
    public class TxFrom
    {
        public string From { get; set; }
        public decimal Amount { get; set; }
    }

    public class TxTo
    {
        public string To { get; set; }
        public decimal Amount { get; set; }
    }

    public enum TxType
    {
        transfer,
        unknown
    }

    public class Tx
    {
        public Asset Asset { get; set; }
        public List<TxFrom> From { get; set; }
        public List<TxTo> To { get; set; }
        public DateTime Date { get; set; }
        public TxType type { get; set; }
        public string Hash { get; set; }
    }
}
