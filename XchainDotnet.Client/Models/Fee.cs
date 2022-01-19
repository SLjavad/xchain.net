using System.Text.Json.Serialization;

namespace XchainDotnet.Client.Models
{
    public enum FeeOption
    {
        average,
        fast,
        fastest
    }



    public enum FeeType
    {
        @byte,
        @base
    }

    public class FeeRates
    {
        [JsonPropertyName("fast")]
        public decimal Fast { get; set; }
        [JsonPropertyName("fastest")]
        public decimal Fastest { get; set; }
        [JsonPropertyName("average")]
        public decimal Average { get; set; }
    }

    public class FeesWithRates
    {
        [JsonPropertyName("rates")]
        public FeeRates Rates { get; set; }
        [JsonPropertyName("fees")]
        public Fees Fees { get; set; }
    }

    public class Fees
    {
        [JsonPropertyName("fast")]
        public decimal Fast { get; set; }
        [JsonPropertyName("fastest")]
        public decimal Fastest { get; set; }
        [JsonPropertyName("average")]
        public decimal Average { get; set; }
        [JsonPropertyName("type")]
        public FeeType Type { get; set; }
    }

    public class FeeParams
    {
        public string Memo { get; set; }
    }
}
