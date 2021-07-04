using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Xchain.net.xchain.client.Models;
using Xchain.net.xchain.thorchain.Models;

namespace Xchain.net.xchain.client
{
    public interface IXchainClient
    {
        // Properties
        string Phrase { get; set; }
        Network Network { get; set; }
        string Address { get; set; }
        ExplorerUrl ExplorerUrl { get; set; }
        // Methods
        string GetExplorerAddressUrl(string address);
        string GetExplorerTxUrl(string txId);
        bool ValidateAddress(string address);
        
        Task<List<Balance>> GetBalance(string address = "", List<Asset> assets = null);
        Task<TxPage> GetTransactions();
        Task<Tx> GetTranasctionData(string txId, string assetAddress = null);
        Task<Fees> GetFees(FeeParams @params = null);
        Task<string> Transfer(TxParams @params);
        void PurgeClient();
    } 
}
