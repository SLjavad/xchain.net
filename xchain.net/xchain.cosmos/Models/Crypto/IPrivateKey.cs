using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchain.net.xchain.cosmos.Models.Crypto
{
    public interface IPrivateKey
    {
        IPublicKey GetPublicKey();
        byte[] ToBuffer();
        string ToBase64();
        byte[] Sign(byte[] message);
    }
}
