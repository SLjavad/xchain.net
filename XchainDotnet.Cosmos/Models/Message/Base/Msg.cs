using System.Text.Json.Serialization;
using XchainDotnet.Cosmos.Utils.JsonConverters;

namespace XchainDotnet.Cosmos.Models.Message.Base
{
    /// <summary>
    /// Base object of the message
    /// </summary>
    [JsonConverter(typeof(MsgJsonConverter))]
    public class Msg
    {
    }
}
