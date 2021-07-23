using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Xchain.net.xchain.cosmos.Models.Crypto
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
                _ => null,
            };
        }

        public override void Write(Utf8JsonWriter writer, IPublicKey value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToBase64()); //TODO: AminoWrapper needed ?
        }
    }

    public interface IPublicKey
    {
        byte[] GetAddress();
        byte[] ToBuffer();
        string ToBase64();
        bool Verify(byte[] signature, byte[] message);
    }
}
