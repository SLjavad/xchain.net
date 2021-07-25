using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xchain.net.xchain.cosmos.Models.Crypto;
using Xchain.net.xchain.cosmos.Utils.JsonConverters;

namespace Xchain.net.xchain.cosmos.Models.Tx
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
