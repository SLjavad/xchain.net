using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Xchain.net.xchain.client.Models;

namespace Xchain.net.xchain.client
{
    public interface IXchainClient
    {
        void SetNetwork(Network network);
        Network GetNetwork();
        string GetExplorerUrl();
        string GetExplorerAddressUrl(string address);
        string GetExplorerTxUrl(string txId);
        Task<bool> ValidateAddress(string address);
        string GetAddress();
        string SetPhrase(string phrase);
        Task<Balance> GetBalance(string address = "", List<Asset> assets = null);
        Task<TxPage> GetTransactions();
        Task<Tx> GetTranasctionData(string txId, string assetAddress);
        Task<Fees> GetFees(FeeParams @params = null);
        Task<string> Transfer(TxParams @params);
        void PurgeClient();
    } 
}
