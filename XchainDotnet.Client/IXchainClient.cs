using System.Collections.Generic;
using System.Threading.Tasks;
using XchainDotnet.Client.Models;

namespace XchainDotnet.Client
{
    public interface IXchainClient
    {
        // Properties
        string Phrase { get; set; }
        Network Network { get; set; }
        string Address { get; set; }
        BaseUrl ExplorerUrl { get; set; }
        // Methods
        string GetExplorerAddressUrl(string address);
        string GetExplorerTxUrl(string txId);
        bool ValidateAddress(string address);

        Task<List<Balance>> GetBalance(string address = "", List<Asset> assets = null);
        Task<TxPage> GetTransactions(TxHistoryParams txHistoryParams = null);
        Task<Tx> GetTranasctionData(string txId, string assetAddress = null);
        Task<Fees> GetFees(FeeParams @params = null);
        Task<string> Transfer(TxParams transferParams);
        void PurgeClient();
    }
}
