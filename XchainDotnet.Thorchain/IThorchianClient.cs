using System.Threading.Tasks;
using XchainDotnet.Cosmos.SDK;
using XchainDotnet.Thorchain.Models;

namespace XchainDotnet.Thorchain
{
    public interface IThorchianClient
    {
        /// <summary>
        /// Client URL object
        /// </summary>
        ClientUrl ClientUrl { get; set; }
        /// <summary>
        /// Get the explorer url for the given node.
        /// </summary>
        /// <param name="node">Node address</param>
        /// <returns>The explorer url for the given node</returns>
        //string GetExplorerNodeUrl(string node);
        /// <summary>
        /// Deposit Method , Transaction with MsgNativeTx
        /// </summary>
        /// <param name="params">The Transaction options</param>
        /// <returns>The trpansaction hash</returns>
        Task<string> Deposit(DepositParam @params);
    }
}
