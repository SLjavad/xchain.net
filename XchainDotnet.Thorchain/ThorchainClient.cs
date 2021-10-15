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
        private string _address;
        private ClientUrl _clientUrl;
        private Network? _network;
        private IPrivateKey _privateKey = null;

        public string Phrase
        {
            get => _phrase;
            set
            {
                if (_phrase != value)
                {
                    if (!XchainCrypto.ValidatePhrase(value))
                    {
                        throw new PhraseNotValidException(value, "Invalid Phrase");
                    }
                    _phrase = value;
                    PrivateKey = null;
                    Address = string.Empty;
                }
            }
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
        public string Address
        {
            get 
            {
                if (string.IsNullOrEmpty(_address))
                {
                    var address = CosmosClient.GetAddressFromPrivKey(PrivateKey);
                    if (string.IsNullOrEmpty(address))
                    {
                        throw new Exception("Address not defined");
                    }
                    _address = address;
                }
                return _address;
            }
            set
            {
                _address = value;
            }
        }
        public ExplorerUrls ExplorerUrls { get; set; }
        /// <summary>
        /// Client Private Key
        /// </summary>
        public IPrivateKey PrivateKey
        {
            get => _privateKey;
            set
            {
                if (_privateKey == null)
                {
                    if (string.IsNullOrEmpty(_phrase))
                    {
                        throw new Exception("phrase not set");
                    }
                    _privateKey = CosmosClient.GetPrivKeyFromMnemonic(_phrase);
                }
            }
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
                Phrase = phrase;
            }
        }

        public string GetExplorerUrl()
        {
            return this.ExplorerUrls.Root.GetExplorerUrlByNetwork(this.Network.Value);
        }

        public async Task<List<Balance>> GetBalance(string address = null, List<Asset> assets = null)
        {
            var rawBalances = await CosmosClient.GetBalance(address ?? Address);
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

        public async Task<Tx> GetTranasctionData(string txId, string assetAddress = null)
        {
            try
            {
                var txResult = await CosmosClient.TxHashGet(txId);
                var action = ThorchainUtils.GetTxType(txResult.Data, "hex");

                List<Tx> txs = new List<Tx>();

                if (action == ThorchainConstantValues.MSG_DEPOSIT)
                {
                    var depositTx = await GetDepositTransaction(txId);
                    depositTx.Date = DateTime.Parse(txResult.TimeStamp);
                    txs.Add(depositTx);
                }
                else
                {
                    var result = ThorchainUtils.GetTxsFromHistory(new List<TxResponse>
                    {
                        txResult
                    }, Network.Value);

                    txs.AddRange(result);
                }

                if (txs.Count == 0)
                {
                    throw new Exception("transaction not found");
                }
                return txs[0];
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
                var address = args?.Address ?? Address;
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
                        var action = ThorchainUtils.GetTxType(tx.TxResult.Data, "base64");
                        return action == ThorchainConstantValues.MSG_DEPOSIT || action == ThorchainConstantValues.MSG_SEND;
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
            Phrase = string.Empty;
            Address = string.Empty;
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

                var assetBalance = await GetBalance(Address, new List<Asset> { transferParams.Asset });
                var fee = await GetFees();
                if (assetBalance.Count == 0 || assetBalance[0].Amount < transferParams.Amount + fee.Average)
                {
                    throw new InsufficientFunds(assetBalance[0].Amount, "insufficient funds");
                }

                var transferResult = await CosmosClient.Transfer(new TransferParams
                {
                    PrivKey = PrivateKey,
                    From = Address,
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
                var assetBalance = await GetBalance(Address, new List<Asset> { depostiParams.Asset });
                if (assetBalance.Count == 0 || assetBalance[0].Amount < depostiParams.Amount + ThorchainConstantValues.DEFAULT_GAS_VALUE)
                {
                    throw new InsufficientFunds(assetBalance[0].Amount, "insufficient funds");
                }

                var signer = Address;

                var msgNativeTx = MsgNativeTx.MsgNativeFromJson(new List<MsgCoin>
            {
                new MsgCoin
                {
                    Amount = depostiParams.Amount.ToString(),
                    Asset = ThorchainUtils.GetDenomWithChain(depostiParams.Asset)
                }
            }, depostiParams.Memo, signer);

                var unsignedStdTx = await BuildDepositTx(msgNativeTx);
                var privateKey = PrivateKey;
                var accAddress = AccAddress.FromBech32(signer);
                var fee = unsignedStdTx.Fee;

                fee.Gas = "10000000";

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

                var unsignedStdTx = StdTx.FromJson(response.Value.Msg, response.Value.Fee, new List<StdSignature>(), "");

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

        public string SetPhrase(string phrase, int walletIndex)
        {
            throw new NotImplementedException();
        }

        public string GetAddress(int? walletIndex)
        {
            throw new NotImplementedException();
        }
    }
}
