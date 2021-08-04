using XchainDotnet.Client.Models;

namespace XchainDotnet.Thorchain.Models
{
    public record AssetRune : Asset
    {
        public override Chain Chain { get => Chain.THOR; }
        public override string Symbol { get => "RUNE"; }
        public override string Ticker { get => "RUNE"; }
    }
}
