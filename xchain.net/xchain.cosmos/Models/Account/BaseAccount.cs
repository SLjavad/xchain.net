using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xchain.net.xchain.cosmos.Models.Address;
using Xchain.net.xchain.cosmos.Models.Crypto;

namespace Xchain.net.xchain.cosmos.Models.Account
{
    public class BaseAccount
    {
        [JsonPropertyName("address")]
        public AccAddress Address { get; set; }
        [JsonPropertyName("public_key")]
        public IPublicKey PublicKey { get; set; }
        [JsonPropertyName("coins")]
        public List<Coin> Coins { get; set; }
        [JsonPropertyName("account_number ")]
        public int AccountNumber { get; set; }
        [JsonPropertyName("sequence")]
        public int Sequence { get; set; }

        public static BaseAccount FromJson(string value)
        {
            var acc = JsonSerializer.Deserialize<BaseAccount>(value, new JsonSerializerOptions
            {
                Converters = {new AccAddressConverter()}
            });
            return acc;
        }
    }

    class AccAddressConverter : JsonConverter<AccAddress>
    {
        public override AccAddress Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return AccAddress.FromBech32(value);
        }

        public override void Write(Utf8JsonWriter writer, AccAddress value, JsonSerializerOptions options)
        {
            string acc = value.ToBech32();
            writer.WriteStringValue(acc);
        }
    }
}


