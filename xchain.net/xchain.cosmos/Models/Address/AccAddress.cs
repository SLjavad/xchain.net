using Bech32;
using NBitcoin.DataEncoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xchain.net.xchain.cosmos.Models.Address.Prefix;
using Xchain.net.xchain.cosmos.Models.Crypto;
using Xchain.net.xchain.cosmos.Utils.JsonConverters;

namespace Xchain.net.xchain.cosmos.Models.Address
{
    [JsonConverter(typeof(AccAddressJsonConvert))]
    public class AccAddress : BaseAddress
    {
        public AccAddress(byte[] value) : base(value)
        {
        }

        public string ToBech32()
        {
            //var words = Bech32Engine.Bytes8to5(this.value);
            try
            {
                var encoded = Bech32Engine.Encode(Bech32Prefix.AccAddr, this.value);
                return encoded;

            }
            catch (Exception ex)
            {
                throw;
            }
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
            return this.ToBech32();
        }
    }
}
