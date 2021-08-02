using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xchain.net.xchain.cosmos.Models.Address;

namespace Xchain.net.xchain.cosmos.Utils.JsonConverters
{
    public class AccAddressJsonConvert : JsonConverter<AccAddress>
    {
        public override AccAddress Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return AccAddress.FromBech32(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, AccAddress value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToBech32());
        }
    }
}
