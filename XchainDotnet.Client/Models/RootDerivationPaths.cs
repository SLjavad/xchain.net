using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace XchainDotnet.Client.Models
{
    public class RootDerivationPaths
    {
        [JsonPropertyName("mainnet")]
        public string Mainnet { get; set; }
        [JsonPropertyName("testnet")]
        public string Testnet { get; set; }

        public string GetByNetwork(Network network)
        {
            return network switch
            {
                Network.mainnet => Mainnet,
                Network.testnet => Testnet,
                _ => "",
            };
        }
    }
}
