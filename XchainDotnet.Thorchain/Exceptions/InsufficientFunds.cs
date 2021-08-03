using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchain.net.xchain.thorchain.Exceptions
{
    public class InsufficientFunds : Exception
    {
        public decimal Amount { get; set; }

        public InsufficientFunds(decimal amount) : base()
        {
            this.Amount = amount;
        }

        public InsufficientFunds(decimal amount, string message) : base(message)
        {
            this.Amount = amount;
        }
    }
}
