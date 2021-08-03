using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchain.net.xchain.client.Models;

namespace Xchain.net.xchain.thorchain.Models
{
    public class ExplorerUrl
    {
        public string Testnet { get; set; }
        public string Mainnet { get; set; }

        public string GetExplorerUrlByNetwork(Network network) => network switch
        {
            Network.mainnet => this.Mainnet,
            Network.testnet => this.Testnet,
            _ => throw new Exception("Network is invalid"),
        };
    }
}
