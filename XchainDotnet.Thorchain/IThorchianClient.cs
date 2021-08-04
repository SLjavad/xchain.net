using System.Threading.Tasks;
using XchainDotnet.Thorchain.Models;

namespace XchainDotnet.Thorchain
{
    public interface IThorchianClient
    {
        ClientUrl ClientUrl { get; set; }
        string GetExplorerNodeUrl(string node);
        Task<string> Deposit(DepositParam @params);

    }
}
