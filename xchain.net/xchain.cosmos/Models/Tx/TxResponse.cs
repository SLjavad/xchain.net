using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xchain.net.xchain.cosmos.Models.Message.Base;

namespace Xchain.net.xchain.cosmos.Models.Tx
{
    public class TxResponse
    {

        [JsonPropertyName("height")]
        public int Height { get; set; }
        [JsonPropertyName("txhash")]
        public string TxHash { get; set; }
        [JsonPropertyName("data")]
        public string Data { get; set; }
        [JsonPropertyName("raw_log")]
        public string RawLog { get; set; }
        [JsonPropertyName("logs")]
        public List<TxLog> Logs { get; set; }
        [JsonPropertyName("gas_wanted")]
        public string GasWanted { get; set; }
        [JsonPropertyName("gas_used")]
        public string GasUsed { get; set; }
        [JsonPropertyName("tx")]
        public object Tx { get; set; }
        [JsonPropertyName("timestamp")]
        public string TimeStamp { get; set; }
    }

    public class Body
    {
        [JsonPropertyName("messages")]
        public List<Msg> Messages { get; set; }
    }
    public class RawTxResponse
    {
        [JsonPropertyName("body")]
        public Body Body { get; set; }
    }
}
