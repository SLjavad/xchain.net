using System;
using System.Text.Json.Serialization;
using XchainDotnet.Cosmos.Models.Address.Prefix;
using XchainDotnet.Cosmos.Models.Crypto;
using XchainDotnet.Cosmos.Utils.JsonConverters;

namespace XchainDotnet.Cosmos.Models.Address
{
    [JsonConverter(typeof(AccAddressJsonConvert))]
    public class AccAddress : BaseAddress
    {
        public AccAddress(byte[] value) : base(value)
        {
        }

        public string ToBech32()
        {
            var encoded = Bech32Engine.Encode(Bech32Prefix.AccAddr, value);
            return encoded;
        }

        public static AccAddress FromBech32(string accAddress)
        {

            Bech32Engine.Decode(accAddress, out string hrp, out byte[] data);
            if (data == null)
            {
                throw new Exception("fromBech32 Decode Error");
            }

            return new AccAddress(data);
        }

        public static new AccAddress FromPublicKey(IPublicKey publicKey)
        {
            return new AccAddress(publicKey.GetAddress());
        }

        public string ToJson()
        {
            return ToBech32();
        }
    }
}
