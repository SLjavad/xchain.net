using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchain.net.xchain.client;
using Xchain.net.xchain.client.Models;
using Xchain.net.xchain.thorchain.Models;

namespace xchain.net.xchain.thorchain
{
    public class ThorchainUtils
    {
        public static ClientUrl GetDefaultClientUrl()
        {
            return new ClientUrl()
            {
                Mainnet = new NodeUrl("https://thornode.thorchain.info", "https://rpc.thorchain.info"),
                Testnet = new NodeUrl("https://testnet.thornode.thorchain.info", "https://testnet.rpc.thorchain.info")
            };
        }

        public static ExplorerUrl GetDefaultExplorerUrl()
        {
            return new ExplorerUrl
            {
                Testnet = "https://testnet.thorchain.net/#",
                Mainnet = "https://thorchain.net/#"
            };
        }

        public static string GetPrefix(Network network) => network switch
        {
            Network.mainnet => "thor",
            Network.testnet => "tthor",
            _ => throw new Exception("Invalid Network"),
        };

        public static Asset GetAsset(string denom)
        {
            if (denom == GetDenom(new AssetRune()))
            {
                return new AssetRune();
            }
            return Utils.AssetFromString($"THOR.{denom.ToUpperInvariant()}");
        }

        public static string GetDenom(Asset asset)
        {
            if (Utils.AssetToString(asset) == Utils.AssetToString(new AssetRune()))
            {
                return "rune";
            }
            else
            {
                return asset.Symbol;
            }
        }
    }
}
