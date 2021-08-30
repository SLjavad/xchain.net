using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace XchainDotnet.Binance.Models
{
    public class MultiTransfer
    {
        [JsonPropertyName("to")]
        public string To { get; set; }
        [JsonPropertyName("coins")]
        public List<Coin> Coins { get; set; }
    }

    public class MultiSendParams
    {
        [JsonPropertyName("walletIndex")]
        public int? WalletIndex { get; set; }
        [JsonPropertyName("transactions")]
        public List<MultiTransfer> Transactions { get; set; }
        [JsonPropertyName("memo")]
        public string Memo { get; set; }
    }
}
