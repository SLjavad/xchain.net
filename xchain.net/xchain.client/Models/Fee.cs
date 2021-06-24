using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchain.net.xchain.client.Models
{
    public enum FeeOptionKey
    {
        average,
        fast,
        fastest
    }

    public enum FeeType
    {
        @byte = 1,
        @base = 2
    }

    public class Fees
    {
        public Dictionary<FeeOptionKey , decimal> FeeOption { get; set; }
        public FeeType Type { get; set; }
    }

    public class FeeParams
    {

    }
}
