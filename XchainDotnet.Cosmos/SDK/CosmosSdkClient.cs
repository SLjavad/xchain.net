using NBitcoin;
using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Cosmos Sdk Client
    /// </summary>
    public class CosmosSdkClient
    {
        public readonly string server;
        public readonly string chainId; //TODO: change to property
        private string prefix;
        private readonly string derivePath;

        private const string BASE_PATH = "https://api.cosmos.network";

        /// <summary>
        /// Cosmos sdk client
        /// </summary>
        /// <param name="server">server address</param>
        /// <param name="chainId">chain id</param>
        /// <param name="prefix">prefix</param>
        /// <param name="derivePath">derivation path</param>
        public CosmosSdkClient(string server, string chainId, string prefix = "cosmos", string derivePath = "44'/118'/0'/0/0")
        {
            this.server = server;
            this.chainId = chainId;
            this.prefix = prefix;
            this.derivePath = derivePath;
        }

        /// <summary>
        /// set default prefixes
        /// </summary>
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

        public void UpdatePrefix(string prefix)
        {
            this.prefix = prefix;
            this.SetPrefix();
        }

        /// <summary>
        /// Get bech32 address from private key
        /// </summary>
        /// <param name="privateKey"></param>
        /// <returns>bech32 address</returns>
        public string GetAddressFromPrivKey(IPrivateKey privateKey)
        {
            SetPrefix();
            return AccAddress.FromPublicKey(privateKey.GetPublicKey()).ToBech32();
        }

        /// <summary>
        /// Get private key from mnemonic
        /// </summary>
        /// <param name="mnemonic">mnemonic phrase</param>
        /// <returns>private key object</returns>
        public IPrivateKey GetPrivKeyFromMnemonic(string mnemonic)
        {
            Mnemonic mnemonic1 = new(mnemonic);
            var key = mnemonic1.DeriveExtKey();
            key = key.Derive(new KeyPath(derivePath));
            return new PrivateKeySecp256k1(key.PrivateKey.ToBytes());
        }

        /// <summary>
        /// Address validation
        /// </summary>
        /// <param name="address">input address</param>
        /// <returns>true or false</returns>
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

        /// <summary>
        /// Get Balances from input address
        /// </summary>
        /// <param name="address">input address</param>
        /// <returns>List of coins belong to the input address</returns>
        public async Task<List<Models.Coin>> GetBalance(string address)
        {
            try
            {
                SetPrefix();
                var apiUrl = $@"{server}/bank/balances/{address}";

                var response = await GlobalHttpClient.HttpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var resString = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<CommonResponse<List<Models.Coin>>>(resString);
                    return result.Result;
                }
                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// Get Tx details
        /// </summary>
        /// <param name="hash">tx hash</param>
        /// <returns><see cref="TxResponse"/></returns>
        public async Task<TxResponse> TxHashGet(string hash)
        {
            try
            {
                SetPrefix();

                var apiUrl = $@"{server}/txs/{hash}";

                var response = await GlobalHttpClient.HttpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var resString = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<TxResponse>(resString);
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

        /// <summary>
        /// Search tx from RPC
        /// </summary>
        /// <param name="searchTxParams">search tx options</param>
        /// <returns><see cref="RPCTxSearchResult"/></returns>
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
                    var rpcResponseString = await response.Content.ReadAsStringAsync();
                    rpcResponse = JsonSerializer.Deserialize<RPCResponse<RPCTxSearchResult>>(rpcResponseString);
                    //rpcResponse = await response.Content.ReadFromJsonAsync<RPCResponse<RPCTxSearchResult>>();
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

        /// <summary>
        /// sing and broadcast tx
        /// </summary>
        /// <param name="unsignedStdTx">input tx</param>
        /// <param name="privateKey">private key</param>
        /// <param name="signer">signer address object</param>
        /// <returns><see cref="BroadcastTxCommitResult"/></returns>
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
                    var resString = await result.Content.ReadAsStringAsync();
                    var response = JsonSerializer.Deserialize<CommonResponse<object>>(resString);
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
                        account = BaseAccount.FromJson((response.Result as BaseAccountResponse).Value); //TODO: change to Aminowrapper
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

        /// <summary>
        /// Trasnfer method
        /// </summary>
        /// <param name="transferParams">Transfer message parameters</param>
        /// <returns><see cref="BroadcastTxCommitResult"/></returns>
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
                                    Amount = transferParams.Amount.ToString(),
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
