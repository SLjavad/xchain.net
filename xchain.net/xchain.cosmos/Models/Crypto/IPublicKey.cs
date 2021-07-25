using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Xchain.net.xchain.cosmos.Models.Crypto
{
    public interface IPublicKey
    {
        byte[] GetAddress();
        byte[] ToBuffer();
        string ToBase64();
        bool Verify(byte[] signature, byte[] message);
    }
}
