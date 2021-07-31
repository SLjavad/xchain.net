using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xchain.net.xchain.cosmos.Models;
using Xchain.net.xchain.cosmos.Models.Message;
using Xchain.net.xchain.cosmos.Models.Message.Base;

namespace Xchain.net.xchain.cosmos.Utils.JsonConverters
{
    public class MsgJsonConverter : JsonConverter<Msg>
    {
        public override Msg Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string type = null;
            JsonElement? value = null;
            
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject || (type != null && value != null))
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
                        value = JsonSerializer.Deserialize<JsonElement>(ref reader);
                        break;
                    default:
                        return JsonSerializer.Deserialize<Msg>(ref reader, options);
                }
            }

            if (type == ConstantValues.THORCHAIN_MSGDEPOSIT)
            {
                var valueString = value.Value.GetRawText();
                MsgDeposit valueObject = JsonSerializer.Deserialize<MsgDeposit>(valueString);
                var msgResult = new AminoWrapper<MsgDeposit>(type, valueObject);
                return msgResult;
            }

            return JsonSerializer.Deserialize<Msg>(ref reader, options);
            
        }

        public override void Write(Utf8JsonWriter writer, Msg value, JsonSerializerOptions options)
        {
            switch (value) // simplify this switch case
            {
                case MsgDeposit msgDeposit:
                    JsonSerializer.Serialize(writer, msgDeposit , options);
                    break;
                case AminoWrapper<MsgDeposit> aminoWrapper:
                    JsonSerializer.Serialize(writer, aminoWrapper , options);
                    break;
                case AminoWrapper<MsgSend> aminoWrapperSend:
                    JsonSerializer.Serialize(writer, aminoWrapperSend , options);
                    break;
                case MsgSend msgSend:
                    JsonSerializer.Serialize(writer, new AminoWrapper<MsgSend>(ConstantValues.THORCHAIN_MSGSEND , msgSend) , options);
                    break;
                default:
                    break;
            }
        }
    }
}
