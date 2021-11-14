using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace XchainDotnet.Cosmos.Models
{
    public class CosmosSDKClientParams
    {
        [JsonPropertyName("server")]
        public string Server { get; set; }
        [JsonPropertyName("chainId")]
        public string ChainId { get; set; }
        [JsonPropertyName("prefix")]
        public string Prefix { get; set; }
    }
}
