using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xchain.net.xchain.cosmos.Models.Message.Base;
using Xchain.net.xchain.cosmos.Models.Tx.Base;
using Xchain.net.xchain.cosmos.Utils.JsonConverters;

namespace Xchain.net.xchain.cosmos.Models.Tx
{
    public class StdTx : ITx
    {
        [JsonPropertyName("msg")]
        public List<Msg> Msg { get; set; } //TODO: AminoWrapper type
        [JsonPropertyName("fee")]
        public StdTxFee Fee { get; set; }
        [JsonPropertyName("memo")]
        public string Memo { get; set; }
        [JsonPropertyName("signatures")]
        public List<StdSignature> Signatures { get; set; }
        [JsonPropertyName("timeout_height")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault | JsonIgnoreCondition.WhenWritingNull)]
        public string TimeoutHeight { get; set; }

        public static StdTx FromJson(List<Msg> msgs , StdTxFee fee , List<StdSignature> signatures , string memo)
        {
            return new StdTx
            {
                Fee = fee,
                Memo = memo,
                Msg = msgs,
                Signatures = signatures
            };
        }

        private JArray NestArraySort(JArray jArray)
        {
            for (int i = 0; i < jArray.Count; i++)
            {
                if (jArray[i] is JObject obj)
                {
                    jArray[i] = new JObject(NestSort(obj.Properties()));
                }
                else if (jArray[i] is JArray jArr)
                {
                    return NestArraySort(jArr);
                }
            }
            return jArray;
        }

        private IEnumerable<JProperty> NestSort(IEnumerable<JProperty> jProperties)
        {
            jProperties = jProperties.OrderBy(x => x.Name);
            foreach (var item in jProperties)
            {
                if (item.Value is JObject)
                {
                    item.Value = new JObject(NestSort((item.Value as JObject).Properties()));
                }
                if (item.Value is JArray jArray)
                {
                    NestArraySort(jArray);
                }
            }
            return jProperties;
        }

        public byte[] GetSignBytes(string chainId , string accountNumber , string sequence)
        {
            StdSignMsg stdSignMsg = new StdSignMsg
            {
                AccountNumber = accountNumber,
                ChainId = chainId,
                Fee = this.Fee,
                Memo = this.Memo,
                Msgs = this.Msg,
                Sequence = sequence
            };

            var serialized = JsonSerializer.Serialize(stdSignMsg);

            JObject jObj = JObject.Parse(serialized);

            var sortedObj = new JObject(
                NestSort(jObj.Properties())
            );

            string sortedJson = sortedObj.ToString(Newtonsoft.Json.Formatting.None);

            var stdSignBytes = Encoding.UTF8.GetBytes(sortedJson);

            return stdSignBytes;
        }
    }
}
