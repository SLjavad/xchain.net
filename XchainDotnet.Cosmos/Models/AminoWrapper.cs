using System.Text.Json.Serialization;
using XchainDotnet.Cosmos.Models.Message.Base;

namespace XchainDotnet.Cosmos.Models
{
    public class AminoWrapper<T> : Msg
    {
        public AminoWrapper(string type, T value)
        {
            Type = type;
            Value = value;
        }

        public AminoWrapper()
        {

        }

        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("value")]
        public T Value { get; set; }

    }
}
