using System;
using XchainDotnet.Client.Models;

namespace XchainDotnet.Client
{
    public class Utils
    {
        /// <summary>
        /// Returns an `Asset` as a string using following naming convention:
        /// `AAA.BBB-CCC`
        /// [chain: `AAA` , 
        /// ticker (optional): `BBB` , 
        /// symbol: `BBB-CCC` or `CCC` (if no ticker available)]
        /// </summary>
        /// <param name="asset">the given asset</param>
        /// <returns>The string from the given asset</returns>
        public static string AssetToString(Asset asset)
        {
            return $"{asset.Chain}.{asset.Symbol}";
        }

        /// <summary>
        /// Type guard to check whether string  is based on type `Chain`
        /// </summary>
        /// <param name="chain">The chain string</param>
        /// <returns>true or false</returns>
        public static bool IsChain(string chain)
        {
            var exists = Enum.IsDefined(typeof(Chain), chain.ToUpperInvariant());
            return exists;
        }

        /// <summary>
        /// Creates an `Asset` by a given string. 
        /// This helper function expects a string with following naming convention:
        /// `AAA.BBB-CCC` where : 
        /// [chain: `AAA` ,
        /// ticker (optional): `BBB` , 
        /// symbol: `BBB-CCC` or `CCC` (if no ticker available)]
        /// <see href="https://docs.thorchain.org/developers/transaction-memos#asset-notation">Official Doc</see>
        /// <para>If the naming convention fails, it returns null</para>
        /// </summary>
        /// <param name="assetString">the given string</param>
        /// <returns>The asset from the given string</returns>
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
