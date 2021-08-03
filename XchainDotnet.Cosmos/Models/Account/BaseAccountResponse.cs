using System.Text.Json.Serialization;

namespace XchainDotnet.Cosmos.Models.Account
{
    public class BaseAccountResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("value")]
        public BaseAccount Value { get; set; }
    }
}
