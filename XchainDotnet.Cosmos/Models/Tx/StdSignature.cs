using System.Text.Json.Serialization;
using XchainDotnet.Cosmos.Models.Crypto;
using XchainDotnet.Cosmos.Utils.JsonConverters;

namespace XchainDotnet.Cosmos.Models.Tx
{
    public class StdSignature
    {
        [JsonPropertyName("pub_key")]
        [JsonConverter(typeof(PublicKeyJsonConverter))]
        public IPublicKey PubKey { get; set; }
        [JsonPropertyName("signature")]
        public string Signature { get; set; }
    }
}
