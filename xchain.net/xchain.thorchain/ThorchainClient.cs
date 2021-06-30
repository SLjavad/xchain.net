using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xchain.net.xchain.thorchain;
using Xchain.net.xchain.client;
using Xchain.net.xchain.client.Models;
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
        private ClientUrl _clientUrl;
        private Network _network;

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
        public string Address { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ExplorerUrl ExplorerUrl { get; set; }

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

        public Task<Balance> GetBalance(string address = "", List<Asset> assets = null)
        {
            throw new NotImplementedException();
        }

        public CosmosSdkClient GetCosmosClient()
        {
            throw new NotImplementedException();
        }

        public string GetExplorerAddressUrl(string address)
        {
            throw new NotImplementedException();
        }

        public string GetExplorerNodeUrl(string node)
        {
            throw new NotImplementedException();
        }

        public string GetExplorerTxUrl(string txId)
        {
            throw new NotImplementedException();
        }

        public Task<Fees> GetFees(FeeParams @params = null)
        {
            throw new NotImplementedException();
        }

        public Task<Tx> GetTranasctionData(string txId, string assetAddress)
        {
            throw new NotImplementedException();
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

        public Task<bool> ValidateAddress(string address)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            this.PurgeClient();
        }
    }
}
