using System.Collections.Generic;
using System.Text.Json.Serialization;
using XchainDotnet.Cosmos.Models.Message.Base;

namespace XchainDotnet.Cosmos.Models.Tx
{
    public class StdSignMsg
    {
        [JsonPropertyName("chain_id")]
        public string ChainId { get; set; }
        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; }
        [JsonPropertyName("sequence")]
        public string Sequence { get; set; }
        [JsonPropertyName("fee")]
        public StdTxFee Fee { get; set; }
        [JsonPropertyName("msgs")]
        public List<Msg> Msgs { get; set; }
        [JsonPropertyName("memo")]
        public string Memo { get; set; }
    }
}
