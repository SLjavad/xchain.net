using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xchain.net.xchain.cosmos.Models;

namespace Xchain.net.xchain.cosmos.Utils.JsonConverters
{
    public class MsgSendNumToStringConverter : JsonConverter<Coin>
    {
        public override Coin Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<Coin>(ref reader , options);
        }

        public override void Write(Utf8JsonWriter writer, Coin value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, new { denom = value.Denom, amount = value.Amount.ToString() });
        }
    }
}
