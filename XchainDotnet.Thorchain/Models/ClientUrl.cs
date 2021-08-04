using System;
using XchainDotnet.Client.Models;

namespace XchainDotnet.Thorchain.Models
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
