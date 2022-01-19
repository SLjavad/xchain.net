using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using XchainDotnet.Client.Models;
using XchainDotnet.Crypto;

namespace XchainDotnet.Client
{
    public abstract class BaseXchainClient : IXchainClient
    {
        private Network? _network;
        private string _phrase;

        protected Chain Chain { get; set; }
        protected RootDerivationPaths RootDerivationPaths { get; set; }

        public string Phrase
        {
            get => _phrase;
        }
        public Network? Network
        {
            get => _network;
            set
            {
                this._network = value ?? throw new Exception("Network must be provided");
            }
        }
        public string Address { get; set; }
        public ExplorerUrl ExplorerUrl { get; set; }


        public BaseXchainClient()
        {

        }

        public BaseXchainClient(Chain chain, XchainClientParams xchainClientParams)
        {
            this.Chain = chain;
            this.Network = xchainClientParams.Network ?? Models.Network.testnet;
            if (xchainClientParams.RootDerivationPaths != null)
            {
                this.RootDerivationPaths = xchainClientParams.RootDerivationPaths;
            }

            if (string.IsNullOrEmpty(xchainClientParams.Phrase))
            {
                if (!ValidateAddress(xchainClientParams.Phrase))
                {
                    throw new Exception("invalid phrase");
                }
                this._phrase = xchainClientParams.Phrase;
            }

        }

        protected async Task<decimal> GetFeeRateFromThorchain()
        {
            var respData = await this.ThornodeApiGet("/inbound_addresses");

            var chainData = respData.Where(x => x.Chain == this.Chain).Select(x => x.GasRate).FirstOrDefault();

            if (string.IsNullOrEmpty(chainData))
            {
                throw new Exception($"Thornode API /inbound_addresses does not contain fees for {this.Chain}");
            }

            return decimal.Parse(chainData);
        }

        protected async Task<List<InboundAddressResponse>> ThornodeApiGet(string endpoint)
        {
            var url = string.Empty;
            switch (this.Network)
            {
                case Models.Network.mainnet:
                    url = ConstVals.MAINNET_THORNODE_API_BASE;
                    break;
                case Models.Network.testnet:
                    url = ConstVals.TESTNET_THORNODE_API_BASE;
                    break;
            }

            var response = await GlobalHttpClient.HttpClient.GetAsync(url + endpoint);

            if (response.IsSuccessStatusCode)
            {
                var resTemp = await response.Content.ReadAsStringAsync();
                var res = JsonSerializer.Deserialize<List<InboundAddressResponse>>(resTemp);
                return res;
            }
            return null;

        }

        protected string GetFullDerivationPath(int walletIndex)
        {
            var res = this.RootDerivationPaths != null ? $"{this.RootDerivationPaths.GetByNetwork(this.Network.Value)}{walletIndex}" : "";
            return res;
        }

        public string SetPhrase(string phrase, int walletIndex)
        {
            if (_phrase != phrase)
            {
                if (!XchainCrypto.ValidatePhrase(phrase))
                {
                    throw new Exception($"{phrase} Invalid Phrase");
                }
                _phrase = phrase;
            }
            return this.GetAddress(walletIndex);
        }

        public void PurgeClient()
        {
            this._phrase = string.Empty;
        }

        public abstract string GetAddress(int? walletIndex);
        public abstract Task<List<Balance>> GetBalance(string address = "", List<Asset> assets = null);
        public abstract string GetExplorerAddressUrl(string address);
        public abstract string GetExplorerTxUrl(string txId);
        public abstract Task<Fees> GetFees(FeeParams @params = null);
        public abstract Task<Tx> GetTranasctionData(string txId, string assetAddress = null);
        public abstract Task<TxPage> GetTransactions(TxHistoryParams txHistoryParams = null);
        public abstract Task<string> Transfer(TxParams transferParams);
        public abstract bool ValidateAddress(string address);
        public abstract string GetExplorerUrl();
    }
}
