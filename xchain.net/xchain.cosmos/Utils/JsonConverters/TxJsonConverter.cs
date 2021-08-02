using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xchain.net.xchain.cosmos.Models;
using Xchain.net.xchain.cosmos.Models.Tx;

namespace Xchain.net.xchain.cosmos.Utils.JsonConverters
{
    public class TxJsonConverter : JsonConverter<object>
    {
        public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string type = null;
            StdTx value = null;
            var tempData = JsonSerializer.Deserialize<JsonElement>(ref reader, options);
            var enumerator = tempData.EnumerateObject();

            if (tempData.TryGetProperty("msg",out JsonElement _))
            {
                var res = JsonSerializer.Deserialize<StdTx>(tempData.GetRawText(), options);
                return res;
            }

            while (enumerator.MoveNext())
            {
                if ((type != null && value != null))
                {
                    break;
                }
                var propName = enumerator.Current.Name;

                switch (propName)
                {
                    case "type":
                        type = enumerator.Current.Value.GetString();
                        break;
                    case "value":
                        value = JsonSerializer.Deserialize<StdTx>(enumerator.Current.Value.GetRawText() , options);
                        break;
                    case "body":
                        Body body = JsonSerializer.Deserialize<Body>(enumerator.Current.Value.GetRawText() , options);
                        RawTxResponse rawTxResponse = new RawTxResponse
                        {
                            Body = body
                        };
                        return rawTxResponse;
                }
            }

            if (!string.IsNullOrEmpty(type) && value != null)
            {
                var aminoStdTx = new AminoWrapper<StdTx>(type, value);
                return aminoStdTx;
            }

            #region OldSolution
            //while (reader.Read())
            //{
            //    if (reader.TokenType == JsonTokenType.EndObject || (type != null && value != null))
            //    {
            //        break;
            //    }
            //    var propName = reader.GetString();
            //    reader.Read();

            //    switch (propName)
            //    {
            //        case "type":
            //            type = reader.GetString();
            //            break;
            //        case "value":
            //            value = JsonSerializer.Deserialize<JsonElement>(ref reader, options);
            //            break;
            //        case "body":
            //            RawTxResponse rawTxResponse = JsonSerializer.Deserialize<RawTxResponse>(ref reader, options);
            //            return rawTxResponse;
            //        default:
            //            var tempData = JsonSerializer.Deserialize<JsonElement>(ref utf8JsonReader);
            //            var rawString = tempData.GetRawText();
            //            StdTx stdTx = JsonSerializer.Deserialize<StdTx>(rawString, options);
            //            return stdTx;
            //    }
            //}

            //switch (type)
            //{
            //    case ConstantValues.COSMOS_STDTX:
            //        {
            //            var valueString = value.Value.GetRawText();
            //            StdTx valueObject = JsonSerializer.Deserialize<StdTx>(valueString , options);
            //            var aminoStdTx = new AminoWrapper<StdTx>(type, valueObject);
            //            return aminoStdTx;
            //        }
            //    default:
            //        break;
            //} 
            #endregion

            return JsonSerializer.Deserialize<object>(tempData.GetRawText(), options);
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            switch (value) // simplify this switch case
            {
                case StdTx stdTx:
                    JsonSerializer.Serialize(writer, stdTx, options);
                    break;
                case AminoWrapper<StdTx> aminoWrapper:
                    JsonSerializer.Serialize(writer, aminoWrapper, options);
                    break;
                case RawTxResponse rawTxResponse:
                    JsonSerializer.Serialize(writer, rawTxResponse, options);
                    break;
                default:
                    break;
            }
        }
    }
}
