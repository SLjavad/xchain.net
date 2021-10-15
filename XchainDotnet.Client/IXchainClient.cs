using System.Collections.Generic;
using System.Threading.Tasks;
using XchainDotnet.Client.Models;

namespace XchainDotnet.Client
{
    public interface IXchainClient
    {
        // Properties
        /// <summary>
        /// Client Phrase
        /// </summary>
        string Phrase { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="phrase"></param>
        /// <param name="walletIndex"></param>
        /// <returns></returns>
        string SetPhrase(string phrase, int walletIndex);
        /// <summary>
        /// Network type (mainnet or testnet)
        /// </summary>
        Network? Network { get; set; }
        /// <summary>
        /// Client address
        /// </summary>
        string Address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="walletIndex"></param>
        /// <returns></returns>
        string GetAddress(int? walletIndex);
        /// <summary>
        /// Get the Explorer URL
        /// </summary>
        string GetExplorerUrl();
        // Methods
        /// <summary>
        /// Get the explorer url for the given address
        /// </summary>
        /// <param name="address">address</param>
        /// <returns>The explorer url for the given address</returns>
        string GetExplorerAddressUrl(string address);
        /// <summary>
        /// Get the explorer url for the given transaction id
        /// </summary>
        /// <param name="txId">transaction id</param>
        /// <returns>The explorer url for the given transaction id</returns>
        string GetExplorerTxUrl(string txId);
        /// <summary>
        /// Validate the given address
        /// </summary>
        /// <param name="address">Address</param>
        /// <returns>validate result</returns>
        bool ValidateAddress(string address);
        /// <summary>
        /// Get the balance of a given address
        /// </summary>
        /// <param name="address">the address. By default, it will return the balance of the current wallet</param>
        /// <param name="assets">If not set, it will return all assets available</param>
        /// <returns>The balance of the address</returns>
        Task<List<Balance>> GetBalance(string address = "", List<Asset> assets = null);
        /// <summary>
        /// Get transaction history of a given address with pagination options.
        /// By default it will return the transaction history of the current wallet.
        /// </summary>
        /// <param name="txHistoryParams">The options to get transaction history</param>
        /// <returns>The transaction history</returns>
        Task<TxPage> GetTransactions(TxHistoryParams txHistoryParams = null);
        /// <summary>
        /// Get the transaction details of a given transaction id
        /// </summary>
        /// <param name="txId">The transaction id</param>
        /// <param name="assetAddress"></param>
        /// <returns>The transaction details of the given transaction id</returns>
        Task<Tx> GetTranasctionData(string txId, string assetAddress = null);
        /// <summary>
        /// Get the fees
        /// </summary>
        /// <param name="params">options for get fees</param>
        /// <returns></returns>
        Task<Fees> GetFees(FeeParams @params = null);
        /// <summary>
        /// Transfer balances with MsgSend
        /// </summary>
        /// <param name="transferParams">The Transfer Options</param>
        /// <returns>The transaction hash</returns>
        Task<string> Transfer(TxParams transferParams);
        /// <summary>
        /// purge client
        /// </summary>
        void PurgeClient();
    }
}
