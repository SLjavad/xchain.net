using System;
using System.Collections.Generic;
using System.Text;
using XchainDotnet.Client.Models;

namespace XchainDotnet.Client
{
    public class FeeRateUtil
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

        public static FeeRates StandardFeeRates(decimal rate)
        {
            return new FeeRates
            {
                Average = (rate / 2),
                Fastest = rate * 5,
                Fast = rate
            };
        }
    }
}
