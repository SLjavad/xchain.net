using System;
using System.Collections.Generic;
using System.Text;
using XchainDotnet.Client.Models;

namespace XchainDotnet.Client
{
    public class FeeUtil
    {
        public static FeeRates SingleFeeRate(decimal rate)
        {
            return new FeeRates
            {
                Average = rate,
                Fast = rate,
                Fastest = rate
            };
        }

        public static Fees SingleFee(FeeType feeType , decimal fee)
        {
            return new Fees
            {
                Average = fee,
                Fast = fee,
                Fastest = fee,
                Type = feeType
            };
        }

        public static FeeRates StandardFeeRates(decimal rate)
        {
            return new FeeRates
            {
                Average = (rate / 2),
                Fastest = rate * 5,
                Fast = rate
            };
        }

        public static Fees StandardFees(FeeType feeType , decimal fee)
        {
            return new Fees
            {
                Average = (fee / 2),
                Fastest = fee * 5,
                Fast = fee,
                Type = feeType
            };
        }

        public static Fees CalcFees(FeeRates feeRates , Func<decimal, string, decimal> calcFee , string memo)
        {
            return new Fees
            {
                Average = calcFee(feeRates.Average, memo),
                Fast = calcFee(feeRates.Fast, memo),
                Fastest = calcFee(feeRates.Fastest, memo),
                Type = FeeType.@byte
            };
        }
    }
}
