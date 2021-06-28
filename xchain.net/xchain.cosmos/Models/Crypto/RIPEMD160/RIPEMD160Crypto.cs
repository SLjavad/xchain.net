using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchain.net.xchain.cosmos.Models.Crypto.RIPEMD160
{
    public abstract class RIPEMD160Crypto : System.Security.Cryptography.HashAlgorithm
    {
        public RIPEMD160Crypto()
        {
        }

        public new static RIPEMD160Crypto Create()
        {
            return new RIPEMD160Managed();
        }

        public new static RIPEMD160Crypto Create(string hashname)
        {
            return new RIPEMD160Managed();
        }
    }
}
