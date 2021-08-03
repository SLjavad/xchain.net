using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using XchainDotnet.Cosmos.Models;
using XchainDotnet.Cosmos.Models.Message;
using XchainDotnet.Cosmos.Models.Message.Base;

namespace XchainDotnet.Cosmos.Utils.JsonConverters
{
    public class MsgJsonConverter : JsonConverter<Msg>
    {
        public override Msg Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string type = null;
            JsonElement? value = null;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject || type != null && value != null)
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
                        value = JsonSerializer.Deserialize<JsonElement>(ref reader, options);
                        break;
                    default:
                        return JsonSerializer.Deserialize<Msg>(ref reader, options);
                }
            }

            switch (type)
            {
                case ConstantValues.THORCHAIN_MSGDEPOSIT:
                    {
                        var valueString = value.Value.GetRawText();
                        MsgDeposit valueObject = JsonSerializer.Deserialize<MsgDeposit>(valueString, options);
                        var msgResult = new AminoWrapper<MsgDeposit>(type, valueObject);
                        return msgResult;
                    }
                case ConstantValues.THORCHAIN_MSGSEND:
                    {
                        var valueString = value.Value.GetRawText();
                        MsgSend valueObject = JsonSerializer.Deserialize<MsgSend>(valueString, options);
                        var msgResult = new AminoWrapper<MsgSend>(type, valueObject);
                        return msgResult;
                    }
                case ConstantValues.THORCHAIN_MSG_MULTI_SEND:
                    {
                        var valueString = value.Value.GetRawText();
                        MsgMultiSend valueObject = JsonSerializer.Deserialize<MsgMultiSend>(valueString, options);
                        var msgResult = new AminoWrapper<MsgMultiSend>(type, valueObject);
                        return msgResult;
                    }
                default:
                    break;
            }

            return JsonSerializer.Deserialize<Msg>(ref reader, options);

        }

        public override void Write(Utf8JsonWriter writer, Msg value, JsonSerializerOptions options)
        {
            switch (value) // simplify this switch case
            {
                case MsgDeposit msgDeposit:
                    JsonSerializer.Serialize(writer, msgDeposit, options);
                    break;
                case AminoWrapper<MsgDeposit> aminoWrapper:
                    JsonSerializer.Serialize(writer, aminoWrapper, options);
                    break;
                case AminoWrapper<MsgSend> aminoWrapperSend:
                    JsonSerializer.Serialize(writer, aminoWrapperSend, options);
                    break;
                case MsgSend msgSend:
                    JsonSerializer.Serialize(writer, new AminoWrapper<MsgSend>(ConstantValues.THORCHAIN_MSGSEND, msgSend), options);
                    break;
                case MsgMultiSend msgMultiSend:
                    JsonSerializer.Serialize(writer, new AminoWrapper<MsgMultiSend>(ConstantValues.THORCHAIN_MSG_MULTI_SEND, msgMultiSend), options);
                    break;
                case AminoWrapper<MsgMultiSend> aminoWrapperMultiSend:
                    JsonSerializer.Serialize(writer, aminoWrapperMultiSend, options);
                    break;
                default:
                    break;
            }
        }
    }
}
