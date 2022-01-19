using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace XchainDotnet.Thorchain.Models
{
    public class ThorchainClientParams
    {
        [JsonPropertyName("clientUrl")]
        public ClientUrl ClientUrl { get; set; }
        [JsonPropertyName("explorerUrls")]
        public ExplorerUrls ExplorerUrls { get; set; }
    }
}
