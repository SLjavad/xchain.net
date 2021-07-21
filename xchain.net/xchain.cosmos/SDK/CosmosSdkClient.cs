﻿using dotnetstandard_bip39;
using NBitcoin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xchain.net.xchain.client;
using Xchain.net.xchain.cosmos.Models;
using Xchain.net.xchain.cosmos.Models.Account;
using Xchain.net.xchain.cosmos.Models.Address;
using Xchain.net.xchain.cosmos.Models.Crypto;
using Xchain.net.xchain.cosmos.Models.RPC;
using Xchain.net.xchain.cosmos.Models.Tx;
using Xchain.net.xchain.crypto;

namespace Xchain.net.xchain.cosmos.SDK
{
    public class CosmosSdkClient
    {
        public  readonly string server;
        public  readonly string chainId; //TODO: change to property
        private readonly string prefix;
        private readonly string derivePath;

        private const string BASE_PATH = "https://api.cosmos.network";

        public CosmosSdkClient(string server , string chainId , string prefix = "cosmos" , string derivePath = "44'/118'/0'/0/0")
        {
            this.server = server;
            this.chainId = chainId;
            this.prefix = prefix;
            this.derivePath = derivePath;
        }

        public void SetPrefix()
        {
            BaseAddress.SetBech32Prefix(
                this.prefix,
                this.prefix + "pub",
                this.prefix + "valoper",
                this.prefix + "valoperpub",
                this.prefix + "valcons",
                this.prefix + "valconspub");
        }

        public string GetAddressFromPrivKey(IPrivateKey privateKey)
        {
            this.SetPrefix();
            return AccAddress.FromPublicKey(privateKey.GetPublicKey()).ToBech32();
        }

        public IPrivateKey GetPrivKeyFromMnemonic(string mnemonic)
        {
            Mnemonic mnemonic1 = new(mnemonic);
            var key = mnemonic1.DeriveExtKey();
            key = key.Derive(new KeyPath(this.derivePath));
            return new PrivateKeySecp256k1(key.PrivateKey.ToBytes());
        }

        public bool CheckAddress(string address)
        {
            try
            {
                this.SetPrefix();

                if (!address.StartsWith(this.prefix))
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
                this.SetPrefix();
                var apiUrl = $@"{this.server}/bank/balances/{address}";

                var response = await GlobalHttpClient.HttpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<Models.Coin>>();
                    return result;
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
                this.SetPrefix();

                var apiUrl = $@"{this.server}/txs/{hash}";

                var response = await GlobalHttpClient.HttpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<TxResponse>();
                    return result;
                }
                throw new Exception("Transaction not found");
            }
            catch (Exception)
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
                    queryParameters.Add($"message.action={searchTxParams.MessageAction}");
                }
                if (!string.IsNullOrEmpty(searchTxParams.MessageSender))
                {
                    queryParameters.Add($"message.sender={searchTxParams.MessageSender}");
                }
                if (!string.IsNullOrEmpty(searchTxParams.TransferSender))
                {
                    queryParameters.Add($"transfer.sender={searchTxParams.TransferSender}");
                }
                if (!string.IsNullOrEmpty(searchTxParams.TransferRecipient))
                {
                    queryParameters.Add($"transfer.recipient={searchTxParams.TransferRecipient}");
                }
                if (searchTxParams.TxMinHeight.HasValue)
                {
                    queryParameters.Add($"tx.height>={searchTxParams.TxMinHeight}");
                }
                if (searchTxParams.TxMaxHeight.HasValue)
                {
                    queryParameters.Add($"tx.height<={searchTxParams.TxMaxHeight}");
                }

                var searchParameter = new List<string>();
                searchParameter.Add($"query={string.Join(" AND ", queryParameters)}");

                if (searchTxParams.Page.HasValue)
                {
                    searchParameter.Add($"page={searchTxParams.Page}");
                }
                if (searchTxParams.Limit.HasValue)
                {
                    searchParameter.Add($"per_page={searchTxParams.Limit}");
                }

                searchParameter.Add("order_by=\"desc\"");

                var response = await GlobalHttpClient.HttpClient.GetAsync($"{searchTxParams.RpcEndpoint}/tx_search?{string.Join('&', searchParameter)}");
                RPCResponse<RPCTxSearchResult> rpcResponse = null;
                if (response.IsSuccessStatusCode)
                {
                    rpcResponse = await response.Content.ReadFromJsonAsync<RPCResponse<RPCTxSearchResult>>();
                }

                return rpcResponse.Result;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BroadcastTxCommitResult> SignAndBroadcast(StdTx unsignedStdTx , IPrivateKey privateKey , AccAddress signer)
        {
            try
            {
                this.SetPrefix();

                // accountAddressGet
                BaseAccount account = null;

                var address = signer.ToBech32();
                var url = $@"{this.server}/auth/accounts/{address}";

                var result = await GlobalHttpClient.HttpClient.GetAsync(url);
                if (result.IsSuccessStatusCode)
                {
                    dynamic response = (await result.Content.ReadFromJsonAsync<dynamic>()).result;
                    if (response is BaseAccount)
                    {
                        account = response;
                    }
                    else
                    {
                        BaseAccount baseAccount = (response as BaseAccountResponse).Value; //TODO: just for test
                        account = BaseAccount.FromJson((response as BaseAccountResponse).Value);
                    }
                }

                var signedStdTx = Auth.SignStdTx(this, privateKey, unsignedStdTx, account.AccountNumber.ToString() , account.Sequence.ToString());

                var postTxResult = await Auth.TxPost(this, new BroadcastTxParams
                {
                    Mode = "block",
                    Tx = signedStdTx
                });

                return postTxResult;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
