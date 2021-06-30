using Bech32;
using NBitcoin.DataEncoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchain.net.xchain.cosmos.Models.Address.Prefix;
using Xchain.net.xchain.cosmos.Models.Crypto;

namespace Xchain.net.xchain.cosmos.Models.Address
{
    public class AccAddress : BaseAddress
    {
        public AccAddress(byte[] value) : base(value)
        {
        }

        public string ToBech32()
        {
            //var words = Bech32Engine.Bytes8to5(this.value);
            var encoded = Bech32Engine.Encode(Bech32Prefix.AccAddr, this.value);
            return encoded;
        }

        public static AccAddress FromBech32(string accAddress)
        {
            string hrp;
            byte[] data;

            Bech32Engine.Decode(accAddress, out hrp, out data);
            if (data == null)
            {
                throw new Exception("fromBech32 Decode Error");
            }

            return new AccAddress(data);
        }

        public static new AccAddress FromPublicKey(PublicKeySecp256k1 publicKey)
        {
            return new AccAddress(publicKey.GetAddress());
        }

        public string ToJson()
        {
            return this.ToBech32();
        }
    }
}
