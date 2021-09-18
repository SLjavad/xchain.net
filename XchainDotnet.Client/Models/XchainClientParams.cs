using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace XchainDotnet.Client.Models
{
    public class XchainClientParams
    {
        [JsonPropertyName("network")]
        public Network Network { get; set; }
        [JsonPropertyName("phrase")]
        public string Phrase { get; set; }
        [JsonPropertyName("rootDerivationPaths")]
        public RootDerivationPaths RootDerivationPaths { get; set; }
    }
}
