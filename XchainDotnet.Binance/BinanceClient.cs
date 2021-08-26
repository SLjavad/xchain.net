using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XchainDotnet.Client;
using XchainDotnet.Client.Models;

namespace XchainDotnet.Binance
{
    public class BinanceClient : IXchainClient
    {
        public string Phrase { get; set; }
        public Network Network { get; set; }
        public string Address { get; set; }
        public ExplorerUrl ExplorerUrl { get; set; }

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
