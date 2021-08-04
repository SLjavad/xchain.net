using NBitcoin;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using XchainDotnet.Client;
using XchainDotnet.Cosmos.Models;
using XchainDotnet.Cosmos.Models.Account;
using XchainDotnet.Cosmos.Models.Address;
using XchainDotnet.Cosmos.Models.Common;
using XchainDotnet.Cosmos.Models.Crypto;
using XchainDotnet.Cosmos.Models.Message;
using XchainDotnet.Cosmos.Models.Message.Base;
using XchainDotnet.Cosmos.Models.RPC;
using XchainDotnet.Cosmos.Models.Tx;

namespace XchainDotnet.Cosmos.SDK
{
    public class CosmosSdkClient
    {
        public readonly string server;
        public readonly string chainId; //TODO: change to property
        private readonly string prefix;
        private readonly string derivePath;

        private const string BASE_PATH = "https://api.cosmos.network";

        public CosmosSdkClient(string server, string chainId, string prefix = "cosmos", string derivePath = "44'/118'/0'/0/0")
        {
            this.server = server;
            this.chainId = chainId;
            this.prefix = prefix;
            this.derivePath = derivePath;
        }

        public void SetPrefix()
        {
            BaseAddress.SetBech32Prefix(
                prefix,
                prefix + "pub",
                prefix + "valoper",
                prefix + "valoperpub",
                prefix + "valcons",
                prefix + "valconspub");
        }

        public string GetAddressFromPrivKey(IPrivateKey privateKey)
        {
            SetPrefix();
            return AccAddress.FromPublicKey(privateKey.GetPublicKey()).ToBech32();
        }

        public IPrivateKey GetPrivKeyFromMnemonic(string mnemonic)
        {
            Mnemonic mnemonic1 = new(mnemonic);
            var key = mnemonic1.DeriveExtKey();
            key = key.Derive(new KeyPath(derivePath));
            return new PrivateKeySecp256k1(key.PrivateKey.ToBytes());
        }

        public bool CheckAddress(string address)
        {
            try
            {
                SetPrefix();

                if (!address.StartsWith(prefix))
                {
                    return false;
                }
                return AccAddress.FromBech32(address).ToBech32() == address;

            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<List<Models.Coin>> GetBalance(string address)
        {
            try
            {
                SetPrefix();
                var apiUrl = $@"{server}/bank/balances/{address}";

                var response = await GlobalHttpClient.HttpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<CommonResponse<List<Models.Coin>>>();
                    return result.Result;
                }
                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<TxResponse> TxHashGet(string hash)
        {
            try
            {
                SetPrefix();

                var apiUrl = $@"{server}/txs/{hash}";

                var response = await GlobalHttpClient.HttpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<TxResponse>();
                    return result;
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        throw new Exception("Transaction not found");
                    }
                    var res = await response.Content.ReadAsStringAsync();
                    throw new Exception(res);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Transaction not found");
            }
        }

        public async Task<RPCTxSearchResult> SearchTxFromRPC(SearchTxParams searchTxParams)
        {
            try
            {
                var queryParameters = new List<string>();
                if (!string.IsNullOrEmpty(searchTxParams.MessageAction))
                {
                    queryParameters.Add($"message.action='{searchTxParams.MessageAction}'");
                }
                if (!string.IsNullOrEmpty(searchTxParams.MessageSender))
                {
                    queryParameters.Add($"message.sender='{searchTxParams.MessageSender}'");
                }
                if (!string.IsNullOrEmpty(searchTxParams.TransferSender))
                {
                    queryParameters.Add($"transfer.sender='{searchTxParams.TransferSender}'");
                }
                if (!string.IsNullOrEmpty(searchTxParams.TransferRecipient))
                {
                    queryParameters.Add($"transfer.recipient='{searchTxParams.TransferRecipient}'");
                }
                if (searchTxParams.TxMinHeight.HasValue)
                {
                    queryParameters.Add($"tx.height>='{searchTxParams.TxMinHeight}'");
                }
                if (searchTxParams.TxMaxHeight.HasValue)
                {
                    queryParameters.Add($"tx.height<='{searchTxParams.TxMaxHeight}'");
                }

                var searchParameter = new List<string>();
                var qParams = string.Join(" AND ", queryParameters);
                searchParameter.Add($"query=\"{qParams}\"");

                if (searchTxParams.Page.HasValue)
                {
                    searchParameter.Add($"page=\"{searchTxParams.Page}\"");
                }
                if (searchTxParams.Limit.HasValue)
                {
                    searchParameter.Add($"per_page=\"{searchTxParams.Limit}\"");
                }

                searchParameter.Add("order_by=\"desc\"");

                var reqParams = string.Join('&', searchParameter);
                var response = await GlobalHttpClient.HttpClient.GetAsync($"{searchTxParams.RpcEndpoint}/tx_search?{reqParams}");
                RPCResponse<RPCTxSearchResult> rpcResponse = null;
                if (response.IsSuccessStatusCode)
                {
                    rpcResponse = await response.Content.ReadFromJsonAsync<RPCResponse<RPCTxSearchResult>>();
                }
                else
                {
                    var res = await response.Content.ReadAsStringAsync();
                    throw new Exception(res);
                }

                return rpcResponse.Result;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BroadcastTxCommitResult> SignAndBroadcast(StdTx unsignedStdTx, IPrivateKey privateKey, AccAddress signer)
        {
            try
            {
                SetPrefix();

                // accountAddressGet
                BaseAccount account = null;

                var address = signer.ToBech32();
                var url = $@"{server}/auth/accounts/{address}";

                var result = await GlobalHttpClient.HttpClient.GetAsync(url);
                if (result.IsSuccessStatusCode)
                {
                    var response = await result.Content.ReadFromJsonAsync<CommonResponse<object>>();
                    if (response.Result is JsonElement jsonElement)
                    {
                        if (jsonElement.TryGetProperty("account_number", out _))
                        {
                            account = JsonSerializer.Deserialize<BaseAccount>(jsonElement.GetRawText());
                        }
                        else
                        {
                            AminoWrapper<BaseAccount> baseAccount = JsonSerializer.Deserialize<AminoWrapper<BaseAccount>>(jsonElement.GetRawText());
                            account = baseAccount.Value;
                        }
                    }
                    else if (response.Result is BaseAccount baseAccount)
                    {
                        account = baseAccount;
                    }
                    else
                    {
                        account = BaseAccount.FromJson((response.Result as BaseAccountResponse).Value);
                    }
                }

                var signedStdTx = Auth.SignStdTx(this, privateKey, unsignedStdTx, account.AccountNumber, account.Sequence);

                var postTxResult = await Auth.TxPost(this, new BroadcastTxParams
                {
                    Mode = "block",
                    Tx = signedStdTx
                });

                return postTxResult;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<BroadcastTxCommitResult> Transfer(TransferParams transferParams)
        {
            try
            {
                SetPrefix();

                var msg = new List<Msg>
                    {
                        new MsgSend
                        {
                            Amount = new List<Models.Coin>
                            {
                                new Models.Coin
                                {
                                    Amount = transferParams.Amount,
                                    Denom = transferParams.Asset
                                }
                            },
                            FromAddress = AccAddress.FromBech32(transferParams.From),
                            ToAddress = AccAddress.FromBech32(transferParams.To)
                        }
                    };

                List<StdSignature> signatures = new();

                var unsignedStdTx = new StdTx
                {
                    Fee = transferParams.Fee,
                    Msg = msg,
                    Memo = transferParams.Memo,
                    Signatures = signatures
                };

                var res = SignAndBroadcast(unsignedStdTx, transferParams.PrivKey, AccAddress.FromBech32(transferParams.From));
                return res;
            }
            catch (Exception ex)
            {
                throw;
            }


        }
    }
}
