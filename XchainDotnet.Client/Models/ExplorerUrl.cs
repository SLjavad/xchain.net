using System;

namespace XchainDotnet.Client.Models
{
    public class ExplorerUrl
    {
        /// <summary>
        /// Testnet address
        /// </summary>
        public string Testnet { get; set; }
        /// <summary>
        /// Mainnet address
        /// </summary>
        public string Mainnet { get; set; }

        /// <summary>
        /// Get Explorer Address According to the network
        /// </summary>
        /// <param name="network">Network type</param>
        /// <returns>Explorer Address</returns>
        public string GetExplorerUrlByNetwork(Network network) => network switch
        {
            Network.mainnet => Mainnet,
            Network.testnet => Testnet,
            _ => throw new Exception("Network is invalid"),
        };
    }
}
