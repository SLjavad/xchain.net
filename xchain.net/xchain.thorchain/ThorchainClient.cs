using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xchain.net.xchain.thorchain;
using Xchain.net.xchain.client;
using Xchain.net.xchain.client.Models;
using Xchain.net.xchain.cosmos.Models.Crypto;
using Xchain.net.xchain.cosmos.SDK;
using Xchain.net.xchain.crypto;
using Xchain.net.xchain.thorchain.Constants;
using Xchain.net.xchain.thorchain.Exceptions;
using Xchain.net.xchain.thorchain.Models;

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
                    if (XcahinCrypto.ValidatePhrase(value))
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
                this.ThorClient = new CosmosSdkClient(value.GetByNetwork(this.Network).Node , "thorchain" , ThorchainUtils.GetPrefix(this.Network) , ConstantValues.DerivePath);
            }
        }

        public Network Network
        {
            get => this._network;
            set
            {
                this._network = value;
                this.ThorClient = new CosmosSdkClient(this.ClientUrl.GetByNetwork(this.Network).Node, "thorchain", ThorchainUtils.GetPrefix(this.Network), ConstantValues.DerivePath);
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
            this.ThorClient = new CosmosSdkClient(this.ClientUrl.GetByNetwork(this.Network).Node, "thorchain", ThorchainUtils.GetPrefix(this.Network), ConstantValues.DerivePath);

            if (!string.IsNullOrEmpty(phrase))
            {
                this.Phrase = phrase;
            }
        }


        public Task<string> Deposit(DepositParam @params)
        {
            throw new NotImplementedException();
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

        public Task<Fees> GetFees(FeeParams @params = null)
        {
            throw new NotImplementedException();
        }

        public async Task<Tx> GetTranasctionData(string txId, string assetAddress = null)
        {
            var txResult = await this.ThorClient.TxHashGet(txId);
        }

        public Task<TxPage> GetTransactions()
        {
            throw new NotImplementedException();
        }

        public void PurgeClient()
        {
            this.Phrase = string.Empty;
            this.Address = string.Empty;
            //this.PrivateKey = null;
        }

        public Task<string> Transfer(TxParams @params)
        {
            throw new NotImplementedException();
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
