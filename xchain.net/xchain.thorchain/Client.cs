using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchain.net.xchain.client;
using Xchain.net.xchain.client.Models;
using Xchain.net.xchain.cosmos.SDK;
using Xchain.net.xchain.crypto;
using Xchain.net.xchain.thorchain.Constants;
using Xchain.net.xchain.thorchain.Exceptions;
using Xchain.net.xchain.thorchain.Models;

namespace Xchain.net.xchain.thorchain
{
    public class Client : IXchainClient , IThorchianClient
    {
        private string _phrase;

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


        public Client(string phrase , ClientUrl clientUrl , ExplorerUrl explorerUrl , Network network = Network.testnet)
        {


            if (!string.IsNullOrEmpty(phrase))
            {
                this.SetPhrase(phrase);
            }
            Phrase = phrase;
        }


        public Task<string> Deposit(DepositParam @params)
        {
            throw new NotImplementedException();
        }

        public string GetAddress()
        {
            throw new NotImplementedException();
        }

        public Task<Balance> GetBalance(string address = "", List<Asset> assets = null)
        {
            throw new NotImplementedException();
        }

        public NodeUrl GetClientUrl()
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

        public string GetExplorerUrl()
        {
            throw new NotImplementedException();
        }

        public Task<Fees> GetFees(FeeParams @params = null)
        {
            throw new NotImplementedException();
        }

        public Network GetNetwork()
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
            throw new NotImplementedException();
        }

        public void SetClientUrl(ClientUrl clientUrl)
        {
            throw new NotImplementedException();
        }

        public void SetExplorerUrl(ExplorerUrl explorerUrl)
        {
            throw new NotImplementedException();
        }

        public void SetNetwork(Network network)
        {
            throw new NotImplementedException();
        }

        public Task<string> Transfer(TxParams @params)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateAddress(string address)
        {
            throw new NotImplementedException();
        }
    }
}
