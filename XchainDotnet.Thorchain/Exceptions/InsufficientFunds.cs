using System;

namespace XchainDotnet.Thorchain.Exceptions
{
    public class InsufficientFunds : Exception
    {
        public decimal Amount { get; set; }

        public InsufficientFunds(decimal amount) : base()
        {
            Amount = amount;
        }

        public InsufficientFunds(decimal amount, string message) : base(message)
        {
            Amount = amount;
        }
    }
}
