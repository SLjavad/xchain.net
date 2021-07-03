using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchain.net.xchain.client.Models;

namespace Xchain.net.xchain.thorchain.Models
{
    public record AssetRune : Asset
    {
        public override Chain Chain { get => Chain.THOR;}
        public override string Symbol { get => "RUNE"; }
        public override string Ticker { get => "RUNE";}
    }
}
