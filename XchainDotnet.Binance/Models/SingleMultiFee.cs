using BinanceClient.Http.Get.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using XchainDotnet.Client.Models;

namespace XchainDotnet.Binance.Models
{
    public class SingleMultiFee
    {
        [JsonPropertyName("single")]
        public Fees Single { get; set; }
        [JsonPropertyName("multi")]
        public Fees Multi { get; set; }
    }
}
