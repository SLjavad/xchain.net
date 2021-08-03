﻿using System.Text.Json.Serialization;

namespace XchainDotnet.Cosmos.Models.Tx
{
    public class TxEventAttribute
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
