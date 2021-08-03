using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchain.net.xchain.client.Models;

namespace Xchain.net.xchain.thorchain.Models
{
    public class ClientUrl
    {
        public NodeUrl Testnet { get; set; }
        public NodeUrl Mainnet { get; set; }

        public NodeUrl GetByNetwork(Network network) => network switch
        {
            Network.mainnet => Mainnet,
            Network.testnet => Testnet,
            _ => throw new Exception("Invalid Network"),
        };
    }
}
