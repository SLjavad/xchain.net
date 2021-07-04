using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xchain.net.xchain.cosmos.Models.Message.Base;
using Xchain.net.xchain.cosmos.Models.Tx.Base;

namespace Xchain.net.xchain.cosmos.Models.Tx
{
    public class StdTx : ITx
    {
        [JsonPropertyName("msg")]
        public List<IMsg> Msg { get; set; } //TODO: AminoWrapper type
        [JsonPropertyName("fee")]
        public StdTxFee Fee { get; set; }
        [JsonPropertyName("memo")]
        public string Memo { get; set; }
        [JsonPropertyName("signature")]
        public List<StdSignature> Signatures { get; set; }
    }
}
