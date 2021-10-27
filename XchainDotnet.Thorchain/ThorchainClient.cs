using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using XchainDotnet.Client;
using XchainDotnet.Client.Models;
using XchainDotnet.Cosmos.Models;
using XchainDotnet.Cosmos.Models.Address;
using XchainDotnet.Cosmos.Models.Crypto;
using XchainDotnet.Cosmos.Models.RPC;
using XchainDotnet.Cosmos.Models.Tx;
using XchainDotnet.Cosmos.SDK;
using XchainDotnet.Crypto;
using XchainDotnet.Thorchain.Constants;
using XchainDotnet.Thorchain.Exceptions;
using XchainDotnet.Thorchain.Models;
using XchainDotnet.Thorchain.Models.Message;
namespace XchainDotnet.Thorchain
{
    public class ThorchainClient : IXchainClient, IThorchianClient, IDisposable
    {
        private string _phrase;
        private ClientUrl _clientUrl;
        private Network? _network;
        private IPrivateKey _privateKey = null;

        public string Phrase
        {
            get => _phrase;
        }

        public ClientUrl ClientUrl
        {
            get
            {
                return _clientUrl;
            }
            set
            {
                _clientUrl = value;
            }
        }

        public Network? Network
        {
            get => _network;
            set
            {
                _network = value;
                this.CosmosClient.UpdatePrefix(ThorchainUtils.GetPrefix(this.Network.Value));
            }
        }
        /// <summary>
        /// Cosmos SDK Client in thorchain client
        /// </summary>
        public CosmosSdkClient CosmosClient { get; set; }
        public ExplorerUrls ExplorerUrls { get; set; }
        /// <summary>
        /// Client Private Key
        /// </summary>
        public IPrivateKey GetPrivateKey(int index = 0)
        {
            if (_privateKey == null)
            {
                if (string.IsNullOrEmpty(_phrase))
                {
                    throw new Exception("phrase not set");
                }
            }
            _privateKey = CosmosClient.GetPrivKeyFromMnemonic(_phrase, this.GetFullDerivationPath(index));
            return _privateKey;
        }

        public RootDerivationPaths RootDerivationPaths { get; set; }

        /// <summary>
        /// Thorchain Client object
        /// <para>if client url and explorer url not provided , they will set by their default value</para>
        /// </summary>
        /// <param name="phrase">input phrase</param>
        /// <param name="clientUrl">Client url</param>
        /// <param name="explorerUrl">Explorer url</param>
        /// <param name="network">network type</param>
        /// <exception cref="PhraseNotValidException">throw exception if phrase is invalid</exception>
        public ThorchainClient(string phrase, ClientUrl clientUrl,
            RootDerivationPaths rootDerivationPaths,
            ExplorerUrls explorerUrls,
            Network network = Client.Models.Network.testnet)
        {
            _network = network;
            ClientUrl = clientUrl ?? ThorchainUtils.GetDefaultClientUrl();
            ExplorerUrls = explorerUrls ?? ThorchainUtils.GetDefaultExplorerUrl();
            CosmosClient = new CosmosSdkClient(ClientUrl.GetByNetwork(Network.Value).Node, "thorchain", ThorchainUtils.GetPrefix(Network.Value), ThorchainConstantValues.DerivePath);

            RootDerivationPaths = rootDerivationPaths ?? new RootDerivationPaths
            {
                Mainnet = "44'/931'/0'/0/",
                Testnet = "44'/931'/0'/0/"
            };

            if (!string.IsNullOrEmpty(phrase))
            {
                _phrase = phrase;
            }
        }

        public string SetPhrase(string phrase, int walletIndex)
        {
            if (this._phrase != phrase)
            {
                if (!XchainCrypto.ValidatePhrase(phrase))
                {
                    throw new Exception("Invalid Phrase");
                }
                this._phrase = phrase;
            }
            return this.GetAddress(walletIndex);
        }

        public string GetAddress(int? walletIndex = 0)
        {
            var address = this.CosmosClient.GetAddressFromMnemonic(this.Phrase, this.GetFullDerivationPath(walletIndex.Value));
            if (string.IsNullOrEmpty(address))
            {
                throw new Exception("address not defined");
            }
            return address;
        }

        public string GetExplorerUrl()
        {
            return this.ExplorerUrls.Root.GetExplorerUrlByNetwork(this.Network.Value);
        }

        public async Task<List<Balance>> GetBalance(string address = null, List<Asset> assets = null)
        {
            var rawBalances = await CosmosClient.GetBalance(address ?? this.GetAddress());
            var balances = rawBalances.Select(x => new Balance
            {
                Amount = decimal.Parse(x.Amount),
                Asset = string.IsNullOrEmpty(x.Denom) ? ThorchainUtils.GetAsset(x.Denom) : new AssetRune()
            }).ToList();

            if (assets != null && assets.Count > 0)
            {
                balances = balances.Where(x => assets.Any(y => Utils.AssetToString(y) == Utils.AssetToString(x.Asset))).ToList();
            }

            return balances;
        }

        public string GetExplorerAddressUrl(string address)
        {
            var explorerAddress = ThorchainUtils.GetExplorerAddressUrl(this.ExplorerUrls, this.Network.Value, address);
            return explorerAddress;
        }

        public string GetExplorerTxUrl(string txId)
        {
            var txUrl = ThorchainUtils.GetExplorerTxUrl(this.ExplorerUrls, this.Network.Value, txId);
            return txUrl;
        }

        public async Task<Fees> GetFees(FeeParams @params = null)
        {
            return await Task.FromResult(ThorchainUtils.GetDefaultFees());
        }

        public string GetFullDerivationPath(int index)
        {
            var res = $"{this.RootDerivationPaths.GetByNetwork(this.Network.Value)}index";
            return res;
        }

        public async Task<Tx> GetTranasctionData(string txId, string assetAddress = null)
        {
            try
            {
                var txResult = await CosmosClient.TxHashGet(txId);
                TxData txData = txResult.Logs != null && txResult.Logs.Count > 0 ? ThorchainUtils.GetDepositTxDataFromLogs(txResult.Logs, assetAddress) : null;
                if (txData == null)
                {
                    throw new Exception($"Failed to get transaction data (tx-hash: {txId})");
                }

                var tx = new Tx
                {
                    Asset = new AssetRune(),
                    Date = DateTime.Parse(txResult.TimeStamp),
                    From = txData.From,
                    Hash = txId,
                    To = txData.To,
                    Type = txData.Type
                };

                return tx; // must be tested 

                //var action = ThorchainUtils.GetTxType(txResult.Data, "hex");

                //List<Tx> txs = new List<Tx>();

                //if (action == ThorchainConstantValues.MSG_DEPOSIT)
                //{
                //    var depositTx = await GetDepositTransaction(txId);
                //    depositTx.Date = DateTime.Parse(txResult.TimeStamp);
                //    txs.Add(depositTx);
                //}
                //else
                //{
                //    var result = ThorchainUtils.GetTxsFromHistory(new List<TxResponse>
                //    {
                //        txResult
                //    }, Network.Value);

                //    txs.AddRange(result);
                //}

                //if (txs.Count == 0)
                //{
                //    throw new Exception("transaction not found");
                //}
                //return txs[0];
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// Get the transaction details of a given transaction id. (from /thorchain/txs/hash)
        /// </summary>
        /// <param name="txId">transaction id</param>
        /// <returns><see cref="Tx"/> Object</returns>
        public async Task<Tx> GetDepositTransaction(string txId)
        {
            try
            {
                var apiUrl = $"{ClientUrl.GetByNetwork(Network.Value).Node}/thorchain/tx/{txId}";
                var response = await GlobalHttpClient.HttpClient.GetAsync(apiUrl);
                TxResult txResult;
                if (response.IsSuccessStatusCode)
                {
                    var resString = await response.Content.ReadAsStringAsync();
                    txResult = JsonSerializer.Deserialize<TxResult>(resString);
                }
                else
                {
                    return null;
                }
                if (txResult == null || txResult.ObservedTx == null)
                {
                    throw new Exception("transaction not found");
                }

                Asset asset = null;
                List<TxFrom> from = new List<TxFrom>();
                List<TxTo> to = new List<TxTo>();

                txResult.ObservedTx.Tx.Coins.ForEach(coin =>
                {
                    from.Add(new TxFrom
                    {
                        Amount = decimal.Parse(coin.Amount),
                        From = txResult.ObservedTx.Tx.FromAddress
                    });
                    to.Add(new TxTo
                    {
                        Amount = decimal.Parse(coin.Amount),
                        To = txResult.ObservedTx.Tx.ToAddress
                    });
                    asset = Utils.AssetFromString(coin.Asset);
                });

                return new Tx
                {
                    Asset = asset ?? new AssetRune(),
                    From = from,
                    Hash = txId,
                    To = to,
                    Type = TxType.transfer
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<TxPage> GetTransactions(TxHistoryParams txHistoryParams = null)
        {
            try
            {
                var prefix = ThorchainUtils.GetPrefix(Network.Value);
                AccAddress.SetBech32Prefix(
                    prefix,
                    prefix + "pub",
                    prefix + "valoper",
                    prefix + "valoperpub",
                    prefix + "valcons",
                    prefix + "valconspub"
                  );

                var args = (TxHistoryParamFilter)txHistoryParams;

                string messageAction = null;
                var address = args?.Address ?? this.GetAddress();
                var offset = args?.Offset ?? 0;
                var limit = args?.Limit ?? 10;
                int? txMinHeight = null;
                int? txMaxHeight = null;

                var txIncomingHistory = (await CosmosClient.SearchTxFromRPC(new SearchTxParams
                {
                    RpcEndpoint = ClientUrl.GetByNetwork(Network.Value).RPC,
                    MessageAction = messageAction,
                    TransferRecipient = address,
                    Limit = ThorchainConstantValues.MAX_TX_COUNT,
                    TxMinHeight = txMinHeight,
                    TxMaxHeight = txMaxHeight
                })).Txs;

                var txOutgoingHistory = (await CosmosClient.SearchTxFromRPC(new SearchTxParams
                {
                    RpcEndpoint = ClientUrl.GetByNetwork(Network.Value).RPC,
                    MessageAction = messageAction,
                    TransferSender = address,
                    Limit = ThorchainConstantValues.MAX_TX_COUNT,
                    TxMinHeight = txMinHeight,
                    TxMaxHeight = txMaxHeight
                })).Txs;

                var history = new List<RPCTxResult>();
                history.AddRange(txIncomingHistory);
                history.AddRange(txOutgoingHistory);

                history.Sort((a, b) =>
                {
                    if (a.Height != b.Height)
                    {
                        return b.Height.CompareTo(a.Height);
                    }
                    if (a.Hash != b.Hash)
                    {
                        return a.Hash.CompareTo(b.Hash);
                    }
                    return 0;
                });

                List<RPCTxResult> accumulator = new List<RPCTxResult>();

                history = history.Aggregate(accumulator, (acc, tx) =>
                    {
                        if (acc.Count > 0)
                        {
                            if (acc[acc.Count - 1].Hash != tx.Hash)
                            {
                                acc.Add(tx);
                            }
                        }
                        else
                        {
                            acc.Add(tx);
                        }
                        return acc;
                    });
                history = history.Where((args?.FilterFn) ?? (tx =>
                    {
                        return tx != null;
                        //var action = ThorchainUtils.GetTxType(tx.TxResult.Data, "base64");
                        //return action == ThorchainConstantValues.MSG_DEPOSIT || action == ThorchainConstantValues.MSG_SEND;
                    })).Take(ThorchainConstantValues.MAX_TX_COUNT).ToList();

                var total = history.Count;

                history = history.Skip(offset).Take(limit).ToList();

                var txs = new List<Tx>();
                foreach (var item in history)
                {
                    var txData = await this.GetTranasctionData(item.Hash);
                    txs.Add(txData);
                }

                return new TxPage
                {
                    Total = total,
                    Txs = txs
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void PurgeClient()
        {
            _phrase = string.Empty;
            //this.PrivateKey = null;
        }

        public async Task<string> Transfer(TxParams transferParams)
        {
            try
            {
                var prefix = ThorchainUtils.GetPrefix(Network.Value);
                AccAddress.SetBech32Prefix(
                    prefix,
                    prefix + "pub",
                    prefix + "valoper",
                    prefix + "valoperpub",
                    prefix + "valcons",
                    prefix + "valconspub"
                  );

                var assetBalance = await GetBalance(this.GetAddress(transferParams.WalletIndex), new List<Asset> { transferParams.Asset });
                var fee = await GetFees();
                if (assetBalance.Count == 0 || assetBalance[0].Amount < transferParams.Amount + fee.Average)
                {
                    throw new InsufficientFunds(assetBalance[0].Amount, "insufficient funds");
                }

                var transferResult = await CosmosClient.Transfer(new TransferParams
                {
                    PrivKey = this.GetPrivateKey(transferParams.WalletIndex),
                    From = this.GetAddress(transferParams.WalletIndex),
                    To = transferParams.Recipient,
                    Amount = transferParams.Amount,
                    Asset = ThorchainUtils.GetDenom(transferParams.Asset),
                    Fee = new StdTxFee
                    {
                        Amount = new List<Coin>(),
                        Gas = ThorchainConstantValues.DEFAULT_GAS_VALUE.ToString()
                    },
                    Memo = transferParams.Memo
                });

                if (!ThorchainUtils.IsBroadcastSuccess(transferResult))
                {
                    throw new Exception($"Failed to broadcast transaction {transferResult.TxHash}");
                }

                return !string.IsNullOrEmpty(transferResult?.TxHash) ? transferResult?.TxHash : "";
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<string> Deposit(DepositParam depostiParams)
        {
            try
            {
                var assetBalance = await GetBalance(this.GetAddress(depostiParams.WalletIndex), new List<Asset> { depostiParams.Asset });
                if (assetBalance.Count == 0 || assetBalance[0].Amount < depostiParams.Amount + ThorchainConstantValues.DEFAULT_GAS_VALUE)
                {
                    throw new InsufficientFunds(assetBalance[0].Amount, "insufficient funds");
                }

                var signer = this.GetAddress(depostiParams.WalletIndex);

                var msgNativeTx = MsgNativeTx.MsgNativeFromJson(new List<MsgCoin>
            {
                new MsgCoin
                {
                    Amount = depostiParams.Amount.ToString(),
                    Asset = ThorchainUtils.GetDenomWithChain(depostiParams.Asset)
                }
            }, depostiParams.Memo, signer);

                var unsignedStdTx = await BuildDepositTx(msgNativeTx);
                var privateKey = this.GetPrivateKey(depostiParams.WalletIndex.Value);
                var accAddress = AccAddress.FromBech32(signer);

                var result = await CosmosClient.SignAndBroadcast(unsignedStdTx, privateKey, accAddress);
                if (result != null && !string.IsNullOrEmpty(result.Data) && !string.IsNullOrEmpty(result.RawLog) && !string.IsNullOrEmpty(result.GasUsed) && !string.IsNullOrEmpty(result.GasWanted))
                {
                    return result.TxHash;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Structure StdTx from MsgNativeTx
        /// </summary>
        /// <param name="msgNativeTx">msgNativeTx</param>
        /// <returns><see cref="StdTx"/> object</returns>
        public async Task<StdTx> BuildDepositTx(MsgNativeTx msgNativeTx)
        {
            try
            {
                var jsonPostData = JsonSerializer.Serialize(new Dictionary<string, object>
                {
                    ["coins"] = msgNativeTx.Coins,
                    ["memo"] = msgNativeTx.Memo,
                    ["base_req"] = new
                    {
                        chain_id = "thorchain",
                        from = msgNativeTx.Signer.ToJson()
                    }
                });

                var contentPostData = new StringContent(jsonPostData);
                var result = await GlobalHttpClient.HttpClient.PostAsync($@"{ClientUrl.GetByNetwork(Network.Value).Node}/thorchain/deposit", contentPostData);

                ThorchainDepositResponse response;

                if (result.IsSuccessStatusCode)
                {
                    var resString = await result.Content.ReadAsStringAsync();
                    response = JsonSerializer.Deserialize<ThorchainDepositResponse>(resString);
                }
                else
                {
                    throw new Exception("Invalid client url");
                }
                if (response.Value == null)
                {
                    throw new Exception("Invalid client url");
                }

                var fee = response.Value?.Fee ?? new StdTxFee
                {
                    Amount = new List<Coin>()
                };

                fee.Gas = ThorchainConstantValues.DEPOSIT_GAS_VALUE.ToString();

                var unsignedStdTx = StdTx.FromJson(response.Value.Msg, fee, new List<StdSignature>(), "");

                return unsignedStdTx;

            }
            catch (Exception ex)
            {
                throw new Exception("invalid client url", ex);
            }
        }

        public bool ValidateAddress(string address)
        {
            return CosmosClient.CheckAddress(address);
        }

        public void Dispose()
        {
            PurgeClient();
        }
    }
}
