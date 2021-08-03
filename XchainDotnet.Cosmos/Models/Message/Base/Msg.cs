using System.Text.Json.Serialization;
using XchainDotnet.Cosmos.Utils.JsonConverters;

namespace XchainDotnet.Cosmos.Models.Message.Base
{
    [JsonConverter(typeof(MsgJsonConverter))]
    public class Msg
    {
    }
}
