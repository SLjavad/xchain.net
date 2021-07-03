using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchain.net.xchain.client.Models;

namespace Xchain.net.xchain.client
{
    public class Utils
    {
        public static string AssetToString(Asset asset)
        {
            return $"{asset.Chain}.{asset.Symbol}";
        }

        public static bool IsChain(string chain)
        {
            var exists = Enum.IsDefined(typeof(Chain), chain.ToUpperInvariant());
            return exists;
        }

        public static Asset AssetFromString(string assetString)
        {
            var data = assetString.Split(".");
            if (data.Length <= 1 || data[1]?.Length < 1)
            {
                return null;
            }

            var chain = data[0];

            if (string.IsNullOrEmpty(chain) || !IsChain(chain))
            {
                return null;
            }

            var symbol = data[1];
            var ticker = symbol.Split("-")[0];

            return new Asset
            {
                Chain = Enum.Parse<Chain>(chain),
                Symbol = symbol,
                Ticker = ticker
            };
        }
    }
}
