using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchain.net.xchain.cosmos.Models.Address.Prefix;
using Xchain.net.xchain.cosmos.Models.Crypto;

namespace Xchain.net.xchain.cosmos.Models.Address
{
    public class BaseAddress
    {
        protected readonly byte[] value;

        public BaseAddress(byte[] value)
        {
            const int addressLen = 20;
            if (value.Length != addressLen)
            {
                throw new Exception("Address must be 20 bytes length.");
            }
            this.value = value;
        }

        public static BaseAddress FromPublicKey(IPublicKey publicKey)
        {
            return new BaseAddress(publicKey.GetAddress());
        }

        public static void SetBech32Prefix(string accAddr , string accPub , string valAddr , string valPub , string consAddr , string consPub)
        {
            Bech32Prefix.AccAddr = accAddr;
            Bech32Prefix.AccPub = accPub;
            Bech32Prefix.ValAddr = valAddr;
            Bech32Prefix.ValPub = valPub;
            Bech32Prefix.ConsAddr = consAddr;
            Bech32Prefix.ConsPub = consPub;
        }
    }
}
