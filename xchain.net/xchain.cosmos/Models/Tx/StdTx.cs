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
        [JsonPropertyName("timeout_height")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault | JsonIgnoreCondition.WhenWritingNull)]
        public string TimeoutHeight { get; set; }

        public static StdTx FromJson(List<IMsg> msgs , StdTxFee fee , List<StdSignature> signatures , string memo)
        {
            return new StdTx
            {
                Fee = fee,
                Memo = memo,
                Msg = msgs,
                Signatures = signatures
            };
        }
    }
}
