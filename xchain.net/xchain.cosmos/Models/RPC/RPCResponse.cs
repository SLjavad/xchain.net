using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Xchain.net.xchain.cosmos.Models.RPC
{
    public class RPCResponse<T>
    {
        [JsonPropertyName("jsonrpc")]
        public string JsonRPC { get; set; }
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("result")]
        public T Result { get; set; }
        //public int Error { get; set; } //TODO: add error field to catch error responses .. but we read response and throw it as Exception
    }
}
