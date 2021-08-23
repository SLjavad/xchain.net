using System.Text.Json.Serialization;
using XchainDotnet.Cosmos.Models.Crypto;
using XchainDotnet.Cosmos.Utils.JsonConverters;

namespace XchainDotnet.Cosmos.Models.Tx
{
    /// <summary>
    /// Signature object
    /// </summary>
    public class StdSignature
    {
        /// <summary>
        /// Public key object
        /// </summary>
        [JsonPropertyName("pub_key")]
        [JsonConverter(typeof(PublicKeyJsonConverter))]
        public IPublicKey PubKey { get; set; }
        /// <summary>
        /// Base64 representation of the signed message
        /// </summary>
        [JsonPropertyName("signature")]
        public string Signature { get; set; }
    }
}
