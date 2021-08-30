using BinanceClient.Http.Get.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XchainDotnet.Binance.Models;
using XchainDotnet.Client.Models;

namespace XchainDotnet.Binance
{
    public interface IXchainBinanceClient
    {
        BinanceClient.Client GetBinanceClient();
        Task<AccountResponse> GetAccount(string address = null, int? index = null);
        Task<Fees> GetMultiSendFees();
        Task<SingleMultiFee> GetSingleAndMultiFees();
        Task<string> MultiSend(MultiSendParams multiSendParams);

    }
}
