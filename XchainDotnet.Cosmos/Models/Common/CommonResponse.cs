using System.Text.Json.Serialization;

namespace XchainDotnet.Cosmos.Models.Common
{
    public class CommonResponse<T>
    {
        [JsonPropertyName("height")]
        public string Height { get; set; }
        [JsonPropertyName("result")]
        public T Result { get; set; }
    }
}
