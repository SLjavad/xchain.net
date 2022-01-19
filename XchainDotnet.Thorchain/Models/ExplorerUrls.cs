using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using XchainDotnet.Client.Models;

namespace XchainDotnet.Thorchain.Models
{
    public class ExplorerUrls
    {
        [JsonPropertyName("root")]
        public ExplorerUrl Root { get; set; }
        [JsonPropertyName("tx")]
        public ExplorerUrl Tx { get; set; }
        [JsonPropertyName("address")]
        public ExplorerUrl Address { get; set; }
    }
}
