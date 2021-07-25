using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xchain.net.xchain.cosmos.Models;
using Xchain.net.xchain.cosmos.Models.Crypto;

namespace Xchain.net.xchain.cosmos.Utils.JsonConverters
{
    public class PublicKeyJsonConverter : JsonConverter<IPublicKey>
    {
        public override IPublicKey Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string type = null;
            string value = null;
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }
                var propName = reader.GetString();
                reader.Read();

                switch (propName)
                {
                    case "type":
                        type = reader.GetString();
                        break;
                    case "value":
                        value = reader.GetString();
                        break;
                }
            }
            return type switch
            {
                ConstantValues.PUBKEY_SECP256K1 => PublicKeySecp256k1.FromJSON(value),
                _ => null
            };
        }

        public override void Write(Utf8JsonWriter writer, IPublicKey value, JsonSerializerOptions options)
        {
            string pub2str = value switch
            {
                PublicKeySecp256k1 => JsonSerializer.Serialize(new AminoWrapper<string>(ConstantValues.PUBKEY_SECP256K1, value.ToBase64())),
                _ => ""
            };
            JsonDocument jsonDocument = JsonDocument.Parse(pub2str);
            jsonDocument.WriteTo(writer);
            JsonSerializer.Serialize(writer);
        }
    }
}
