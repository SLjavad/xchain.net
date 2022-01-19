using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XchainDotnet.Client.Models;

namespace XchainDotnet.Client
{
    public abstract class UTXOClient : BaseXchainClient
    {

        protected abstract Task<int> GetSuggestedFeeRate();
        protected abstract decimal CalcFee(decimal feeRate, string memo);
        public async Task<FeesWithRates> GetFeesWithRates(string memo)
        {
            var rates = await GetFeeRates();
            var res = new FeesWithRates
            {
                Fees = FeeUtil.CalcFees(rates, CalcFee, memo),
                Rates = rates
            };

            return res;
        } 

        public async Task<FeeRates> GetFeeRates()
        {
            decimal feeRate;
            try
            {
                feeRate = await this.GetFeeRateFromThorchain();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Rate lookup via Thorchain failed: {ex.Message}");
                feeRate = await this.GetSuggestedFeeRate();
            }

            var res = FeeUtil.StandardFeeRates(feeRate);
            return res;
        }

        public override async Task<Fees> GetFees(FeeParams @params = null)
        {
            var feeWithRate = await this.GetFeesWithRates(@params.Memo);
            return feeWithRate.Fees;
        }

        public abstract override string GetAddress(int? walletIndex);

        public abstract override Task<List<Balance>> GetBalance(string address = "", List<Asset> assets = null);

        public abstract override string GetExplorerAddressUrl(string address);

        public abstract override string GetExplorerTxUrl(string txId);

        public abstract override Task<Tx> GetTranasctionData(string txId, string assetAddress = null);

        public abstract override Task<TxPage> GetTransactions(TxHistoryParams txHistoryParams = null);

        public abstract override Task<string> Transfer(TxParams transferParams);

        public abstract override bool ValidateAddress(string address);
    }
}
