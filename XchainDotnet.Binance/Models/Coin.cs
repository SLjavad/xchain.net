using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using XchainDotnet.Client.Models;

namespace XchainDotnet.Binance.Models
{
    public class Coin
    {
        [JsonPropertyName("asset")]
        public Asset Asset { get; set; }
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
    }
}
