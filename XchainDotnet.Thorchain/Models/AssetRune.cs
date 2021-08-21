using XchainDotnet.Client.Models;

namespace XchainDotnet.Thorchain.Models
{
    /// <summary>
    /// Rune Asset object
    /// </summary>
    public class AssetRune : Asset
    {
        /// <summary>
        /// Asset Chain type
        /// </summary>
        public override Chain Chain { get => Chain.THOR; }
        /// <summary>
        /// Asset Symbol
        /// </summary>
        public override string Symbol { get => "RUNE"; }
        /// <summary>
        /// Asset Ticker
        /// </summary>
        public override string Ticker { get => "RUNE"; }
    }
}
