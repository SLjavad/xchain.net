using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace XchainDotnet.Thorchain.Models
{
    public class TransferData
    {
        [JsonPropertyName("sender")]
        public string Sender { get; set; }
        [JsonPropertyName("recipient")]
        public string Recipient { get; set; }
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
    }
}
