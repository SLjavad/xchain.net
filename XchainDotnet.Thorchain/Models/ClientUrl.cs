using System;
using XchainDotnet.Client.Models;

namespace XchainDotnet.Thorchain.Models
{
    /// <summary>
    /// Object representation Client URL
    /// </summary>
    public class ClientUrl
    {
        /// <summary>
        /// Testnet node url
        /// </summary>
        public NodeUrl Testnet { get; set; }
        /// <summary>
        /// Mainnet Node url
        /// </summary>
        public NodeUrl Mainnet { get; set; }

        /// <summary>
        /// Get node url based on network type
        /// </summary>
        /// <param name="network">network type</param>
        /// <returns>node url</returns>
        public NodeUrl GetByNetwork(Network network) => network switch
        {
            Network.mainnet => Mainnet,
            Network.testnet => Testnet,
            _ => throw new Exception("Invalid Network"),
        };
    }
}
