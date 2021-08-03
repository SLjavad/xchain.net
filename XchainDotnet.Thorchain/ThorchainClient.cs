using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using xchain.net.xchain.thorchain;
using Xchain.net.xchain.client;
using Xchain.net.xchain.client.Models;
using Xchain.net.xchain.cosmos.Models;
using Xchain.net.xchain.cosmos.Models.Address;
using Xchain.net.xchain.cosmos.Models.Crypto;
using Xchain.net.xchain.cosmos.Models.RPC;
using Xchain.net.xchain.cosmos.Models.Tx;
using Xchain.net.xchain.cosmos.SDK;
using Xchain.net.xchain.crypto;
using Xchain.net.xchain.thorchain.Constants;
using Xchain.net.xchain.thorchain.Exceptions;
using Xchain.net.xchain.thorchain.Models;
using Xchain.net.xchain.thorchain.Models.Message;

namespace Xchain.net.xchain.thorchain
{
    public class ThorchainClient : IXchainClient , IThorchianClient , IDisposable
    {
        private string _phrase;
        private string _address;
        private ClientUrl _clientUrl;
        private Network _network;
        private IPrivateKey _privateKey = null;

        public string Phrase 
        {
            get => this._phrase;
            set
            {
                if (this._phrase != value)
                {
                    if (!XcahinCrypto.ValidatePhrase(value))
                    {
                        throw new PhraseNotValidException(value , "Invalid Phrase");
                    }
                    this._phrase = value;
                    this.PrivateKey = null;
                    this.Address = string.Empty;
                }
            }
        }

        public ClientUrl ClientUrl
        {
            get
            {
                return this._clientUrl;
            }
            set
            {
                this._clientUrl = value;
                this.ThorClient = new CosmosSdkClient(value.GetByNetwork(this.Network).Node , "thorchain" , ThorchainUtils.GetPrefix(this.Network) , ThorchainConstantValues.DerivePath);
            }
        }

        public Network Network
        {
            get => this._network;
            set
            {
                this._network = value;
                this.ThorClient = new CosmosSdkClient(this.ClientUrl.GetByNetwork(this.Network).Node, "thorchain", ThorchainUtils.GetPrefix(this.Network), ThorchainConstantValues.DerivePath);
                this.Address = string.Empty;
            }
        }
        public CosmosSdkClient ThorClient { get; set; }
        public string Address
        {
            get => this._address;
            set
            {
                if (string.IsNullOrEmpty(this._address))
                {
                    var address = this.ThorClient.GetAddressFromPrivKey(this.PrivateKey);
                    if (string.IsNullOrEmpty(address))
                    {
                        throw new Exception("Address not defined");
                    }
                    this._address = address;
                }
            }
        }
        public ExplorerUrl ExplorerUrl { get; set; }
        public IPrivateKey PrivateKey
        {
            get => this._privateKey;
            set
            {
                if (this._privateKey == null)
                {
                    if (string.IsNullOrEmpty(this._phrase))
                    {
                        throw new Exception("phrase not set");
                    }
                    this._privateKey = this.ThorClient.GetPrivKeyFromMnemonic(this._phrase);
                }
            }
        }

        public ThorchainClient(string phrase , ClientUrl clientUrl , ExplorerUrl explorerUrl , Network network = Network.testnet)
        {
            this._network = network;
            this.ClientUrl = clientUrl ?? ThorchainUtils.GetDefaultClientUrl();
            this.ExplorerUrl = explorerUrl ?? ThorchainUtils.GetDefaultExplorerUrl();
            this.ThorClient = new CosmosSdkClient(this.ClientUrl.GetByNetwork(this.Network).Node, "thorchain", ThorchainUtils.GetPrefix(this.Network), ThorchainConstantValues.DerivePath);

            if (!string.IsNullOrEmpty(phrase))
            {
                this.Phrase = phrase;
            }
        }

        public async Task<List<Balance>> GetBalance(string address = null, List<Asset> assets = null)
        {
            var rawBalances = await this.ThorClient.GetBalance(address ?? this.Address);
            var balances = rawBalances.Select(x => new Balance
            {
               Amount = x.Amount,
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
            var explorerAddress = $@"{this.ExplorerUrl.GetExplorerUrlByNetwork(this.Network)}/address/{address}";
            return explorerAddress;
        }

        public string GetExplorerNodeUrl(string node)
        {
            var explorerNodeUr = $@"{this.ExplorerUrl.GetExplorerUrlByNetwork(this.Network)}/nodes/{node}";
            return explorerNodeUr;
        }

        public string GetExplorerTxUrl(string txId)
        {
            var txUrl = $@"{this.ExplorerUrl.GetExplorerUrlByNetwork(this.Network)}/txs/{txId}";
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
                var txResult = await this.ThorClient.TxHashGet(txId);
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
                    var result = ThorchainUtils.GetTxFromHistory(new List<cosmos.Models.Tx.TxResponse>
                    {
                        txResult
                    }, this.Network);

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

        public async Task<Tx> GetDepositTransaction(string txId)
        {
            try
            {
                var apiUrl = $"{this.ClientUrl.GetByNetwork(this.Network).Node}/thorchain/tx/{txId}";
                var response = await GlobalHttpClient.HttpClient.GetAsync(apiUrl);
                TxResult txResult;
                if (response.IsSuccessStatusCode)
                {
                    txResult = await response.Content.ReadFromJsonAsync<TxResult>();
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
                var prefix = ThorchainUtils.GetPrefix(this.Network);
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
                var address = args?.Address ?? this.Address;
                var offset = args?.Offset ?? 0;
                var limit = args?.Limit ?? 10;
                int? txMinHeight = null;
                int? txMaxHeight = null;

                var txIncomingHistory = (await ThorClient.SearchTxFromRPC(new SearchTxParams
                {
                    RpcEndpoint = this.ClientUrl.GetByNetwork(this.Network).RPC,
                    MessageAction = messageAction,
                    TransferRecipient = address,
                    Limit = ThorchainConstantValues.MAX_TX_COUNT,
                    TxMinHeight = txMinHeight,
                    TxMaxHeight = txMaxHeight
                })).Txs;

                var txOutgoingHistory = (await ThorClient.SearchTxFromRPC(new SearchTxParams
                {
                    RpcEndpoint = this.ClientUrl.GetByNetwork(this.Network).RPC,
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
            this.Phrase = string.Empty;
            this.Address = string.Empty;
            //this.PrivateKey = null;
        }

        public async Task<string> Transfer(TxParams transferParams)
        {
            try
            {
                var prefix = ThorchainUtils.GetPrefix(this.Network);
                AccAddress.SetBech32Prefix(
                    prefix,
                    prefix + "pub",
                    prefix + "valoper",
                    prefix + "valoperpub",
                    prefix + "valcons",
                    prefix + "valconspub"
                  );

                var assetBalance = await this.GetBalance(this.Address, new List<Asset> { transferParams.Asset });
                var fee = await this.GetFees();
                if (assetBalance.Count == 0 || assetBalance[0].Amount < (transferParams.Amount + fee.Average))
                {
                    throw new InsufficientFunds(assetBalance[0].Amount, "insufficient funds");
                }

                var transferResult = await this.ThorClient.Transfer(new TransferParams
                {
                    PrivKey = this.PrivateKey,
                    From = this.Address,
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
                var assetBalance = await this.GetBalance(this.Address, new List<Asset> { depostiParams.Asset });
                if (assetBalance.Count == 0 || assetBalance[0].Amount < (depostiParams.Amount + ThorchainConstantValues.DEFAULT_GAS_VALUE))
                {
                    throw new InsufficientFunds(assetBalance[0].Amount, "insufficient funds");
                }

                var signer = this.Address;

                var msgNativeTx = MsgNativeTx.MsgNativeFromJson(new List<MsgCoin>
            {
                new MsgCoin
                {
                    Amount = depostiParams.Amount.ToString(),
                    Asset = ThorchainUtils.GetDenomWithChain(depostiParams.Asset)
                }
            }, depostiParams.Memo, signer);

                var unsignedStdTx = await this.BuildDepositTx(msgNativeTx);
                var privateKey = this.PrivateKey;
                var accAddress = AccAddress.FromBech32(signer);
                var fee = unsignedStdTx.Fee;

                fee.Gas = "10000000";

                var result = await this.ThorClient.SignAndBroadcast(unsignedStdTx, privateKey, accAddress);
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

        public async Task<StdTx> BuildDepositTx(MsgNativeTx msgNativeTx)
        {
            try
            {
                var jsonPostData = JsonSerializer.Serialize(new Dictionary<string,object>
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
                var result = await GlobalHttpClient.HttpClient.PostAsync($@"{this.ClientUrl.GetByNetwork(this.Network).Node}/thorchain/deposit", contentPostData);

                ThorchainDepositResponse response;

                if (result.IsSuccessStatusCode)
                {
                    response = await result.Content.ReadFromJsonAsync<ThorchainDepositResponse>();
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
            return this.ThorClient.CheckAddress(address);
        }

        public void Dispose()
        {
            this.PurgeClient();
        }
    }
}
