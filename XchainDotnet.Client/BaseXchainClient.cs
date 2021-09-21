using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using XchainDotnet.Client.Models;

namespace XchainDotnet.Client
{
    public abstract class BaseXchainClient : IXchainClient
    {
        private Network? _network;

        protected Chain Chain { get; set; }
        protected RootDerivationPaths RootDerivationPaths { get; set; }

        public string Phrase { get; set; }
        public Network? Network {
            get => _network;
            set
            {
                this._network = value ?? throw new Exception("Network must be provided");
            } 
        }
        public string Address { get; set; }
        public ExplorerUrl ExplorerUrl { get; set; }


        public BaseXchainClient(Chain chain , XchainClientParams xchainClientParams)
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
                this.Phrase = xchainClientParams.Phrase;
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
                    url =  ConstVals.MAINNET_THORNODE_API_BASE;
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

        public string GetAddress(int? walletIndex)
        {
            throw new NotImplementedException();
        }

        public Task<List<Balance>> GetBalance(string address = "", List<Asset> assets = null)
        {
            throw new NotImplementedException();
        }

        public string GetExplorerAddressUrl(string address)
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

        public Task<Tx> GetTranasctionData(string txId, string assetAddress = null)
        {
            throw new NotImplementedException();
        }

        public Task<TxPage> GetTransactions(TxHistoryParams txHistoryParams = null)
        {
            throw new NotImplementedException();
        }

        public void PurgeClient()
        {
            throw new NotImplementedException();
        }

        public string SetPhrase(string phrase, int walletIndex)
        {
            throw new NotImplementedException();
        }

        public Task<string> Transfer(TxParams transferParams)
        {
            throw new NotImplementedException();
        }

        public bool ValidateAddress(string address)
        {
            throw new NotImplementedException();
        }
    }
}
