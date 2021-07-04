using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xchain.net.xchain.cosmos.Models.Crypto;

namespace Xchain.net.xchain.cosmos.Models.Tx
{
    public class StdSignature
    {
        [JsonPropertyName("pub_key")]
        public IPublicKey PubKey { get; set; }
        [JsonPropertyName("signature")]
        public string Signature { get; set; }
    }
}
