using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Xchain.net.xchain.cosmos.Models.Common
{
    class CommonResponse<T>
    {
        [JsonPropertyName("height")]
        public string Height { get; set; }
        [JsonPropertyName("result")]
        public T Result { get; set; }
    }
}
