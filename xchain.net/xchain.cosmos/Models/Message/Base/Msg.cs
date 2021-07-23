using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xchain.net.xchain.cosmos.Utils.JsonConverters;

namespace Xchain.net.xchain.cosmos.Models.Message.Base
{
    [JsonConverter(typeof(MsgJsonConverter))]
    public class Msg
    {
    }
}
