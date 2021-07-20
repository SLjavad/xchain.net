using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xchain.net.xchain.cosmos.Models.Message.Base;

namespace Xchain.net.xchain.cosmos.Models.Tx
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
        public List<IMsg> Msgs { get; set; }
        [JsonPropertyName("memo")]
        public string Memo { get; set; }
    }
}
