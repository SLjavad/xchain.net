using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchain.net.xchain.cosmos.Models.Address
{
    public class BaseAddress
    {
        private readonly byte[] value;

        public BaseAddress(byte[] value)
        {
            const int addressLen = 20;
            if (value.Length != addressLen)
            {
                throw new Exception("Address must be 20 bytes length.");
            }
            this.value = value;
        }
    }
}
