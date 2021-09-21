using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace XchainDotnet.Client.Models
{
    public class InboundAddressResponse
    {
        [JsonPropertyName("chain")]
        public Chain Chain { get; set; }
        [JsonPropertyName("pub_key")]
        public string PubKey { get; set; }
        [JsonPropertyName("address")]
        public string Address { get; set; }
        [JsonPropertyName("halted")]
        public bool Halted { get; set; }
        [JsonPropertyName("gas_rate")]
        public string GasRate { get; set; }
    }
}
