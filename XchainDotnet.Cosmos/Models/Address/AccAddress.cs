using System;
using System.Text.Json.Serialization;
using XchainDotnet.Cosmos.Models.Address.Prefix;
using XchainDotnet.Cosmos.Models.Crypto;
using XchainDotnet.Cosmos.Utils.JsonConverters;

namespace XchainDotnet.Cosmos.Models.Address
{
    /// <summary>
    /// Account Address object
    /// </summary>
    [JsonConverter(typeof(AccAddressJsonConvert))]
    public class AccAddress : BaseAddress
    {
        /// <summary>
        /// Account Address object
        /// </summary>
        /// <param name="value">byte value of account address</param>
        public AccAddress(byte[] value) : base(value)
        {
        }

        /// <summary>
        /// Get Account Address as string (Bech32 representation)
        /// </summary>
        /// <returns>Bech32 representation of account address</returns>
        public string ToBech32()
        {
            var encoded = Bech32Engine.Encode(Bech32Prefix.AccAddr, value);
            return encoded;
        }

        /// <summary>
        /// Get Account Address Object from its bech32 string
        /// </summary>
        /// <param name="accAddress">bech32 string address</param>
        /// <returns>Account Address object</returns>
        public static AccAddress FromBech32(string accAddress)
        {

            Bech32Engine.Decode(accAddress, out string hrp, out byte[] data);
            if (data == null)
            {
                throw new Exception("fromBech32 Decode Error");
            }

            return new AccAddress(data);
        }

        /// <summary>
        /// Get Account address from public key
        /// </summary>
        /// <param name="publicKey">public key</param>
        /// <returns>Account Address object</returns>
        public static new AccAddress FromPublicKey(IPublicKey publicKey)
        {
            return new AccAddress(publicKey.GetAddress());
        }

        /// <summary>
        /// ToBech32
        /// </summary>
        /// <returns>Bech32 address</returns>
        public string ToJson()
        {
            return ToBech32();
        }
    }
}
