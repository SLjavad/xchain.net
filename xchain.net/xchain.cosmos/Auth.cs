using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xchain.net.xchain.client;
using Xchain.net.xchain.cosmos.Models.Crypto;
using Xchain.net.xchain.cosmos.Models.Tx;
using Xchain.net.xchain.cosmos.SDK;
using Xchain.net.xchain.cosmos.Utils.JsonConverters;

namespace Xchain.net.xchain.cosmos
{
    public class Auth
    {
        public static StdTx SignStdTx(CosmosSdkClient cosmosSdkClient, IPrivateKey privateKey, StdTx stdTx, string accountNumber, string sequence)
        {
            var signBytes = stdTx.GetSignBytes(cosmosSdkClient.chainId, accountNumber, sequence);

            var sign = new StdSignature
            {
                PubKey = privateKey.GetPublicKey(),
                Signature = Convert.ToBase64String(privateKey.Sign(signBytes))
            };

            var newStdTx = new StdTx
            {
                Msg = stdTx.Msg,
                Fee = stdTx.Fee,
                Memo = stdTx.Memo,
                Signatures = stdTx.Signatures != null ? stdTx.Signatures.Append(sign).ToList() : new List<StdSignature> { sign }
            };

            return newStdTx;
        }

        public static async Task<BroadcastTxCommitResult> TxPost(CosmosSdkClient cosmosSdkClient , BroadcastTxParams broadcastTxParams)
        {
            try
            {
                var url = $"{cosmosSdkClient.server}/txs";

                var serializedParams = JsonSerializer.Serialize(broadcastTxParams ,new JsonSerializerOptions
                {
                    Converters =
                    {
                        new MsgSendNumToStringConverter()
                    }
                });

                var dataToPost = new StringContent(serializedParams);

                var response = await GlobalHttpClient.HttpClient.PostAsync(url, dataToPost);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<BroadcastTxCommitResult>();
                    return result;
                }
                else
                {
                    var result = await response.Content.ReadAsStringAsync();
                    throw new Exception(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
