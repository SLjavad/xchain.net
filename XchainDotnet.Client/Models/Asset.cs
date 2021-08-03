using System.Text.Json.Serialization;

namespace XchainDotnet.Client.Models
{
    public record Asset
    {
        [JsonPropertyName("chain")]
        public virtual Chain Chain { get; set; }
        [JsonPropertyName("symbol")]
        public virtual string Symbol { get; set; }
        [JsonPropertyName("ticker")]
        public virtual string Ticker { get; set; }
    }
}
