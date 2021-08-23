using System.Text.Json.Serialization;

namespace XchainDotnet.Cosmos.Models.Account
{
    /// <summary>
    /// Base Account Response
    /// </summary>
    public class BaseAccountResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("value")]
        public BaseAccount Value { get; set; }
    }
}
