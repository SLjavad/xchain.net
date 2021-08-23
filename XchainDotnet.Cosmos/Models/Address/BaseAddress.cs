using System;
using XchainDotnet.Cosmos.Models.Address.Prefix;
using XchainDotnet.Cosmos.Models.Crypto;

namespace XchainDotnet.Cosmos.Models.Address
{
    /// <summary>
    /// Base Address
    /// </summary>
    public class BaseAddress
    {
        protected readonly byte[] value;

        /// <summary>
        /// Base Address
        /// </summary>
        /// <param name="value">input value as byte array. must be 20 bytes length</param>
        public BaseAddress(byte[] value)
        {
            const int addressLen = 20;
            if (value.Length != addressLen)
            {
                throw new Exception("Address must be 20 bytes length.");
            }
            this.value = value;
        }

        /// <summary>
        /// Get Address from public key
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static BaseAddress FromPublicKey(IPublicKey publicKey)
        {
            return new BaseAddress(publicKey.GetAddress());
        }

        public static void SetBech32Prefix(string accAddr, string accPub, string valAddr, string valPub, string consAddr, string consPub)
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
