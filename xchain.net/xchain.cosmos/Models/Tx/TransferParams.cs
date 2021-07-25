﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xchain.net.xchain.cosmos.Models.Crypto;

namespace Xchain.net.xchain.cosmos.Models.Tx
{
    public class TransferParams
    {
        [JsonPropertyName("privkey")]
        public IPrivateKey PrivKey { get; set; }
        [JsonPropertyName("from")]
        public string From { get; set; }
        [JsonPropertyName("to")]
        public string To { get; set; }
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        [JsonPropertyName("asset")]
        public string Asset { get; set; }
        [JsonPropertyName("memo")]
        public string Memo { get; set; }
        [JsonPropertyName("fee")]
        public StdTxFee Fee { get; set; }
    }
}
