using System;

namespace XchainDotnet.Client.Models
{
    public class ExplorerUrl
    {
        public string Testnet { get; set; }
        public string Mainnet { get; set; }

        public string GetExplorerUrlByNetwork(Network network) => network switch
        {
            Network.mainnet => Mainnet,
            Network.testnet => Testnet,
            _ => throw new Exception("Network is invalid"),
        };
    }
}
