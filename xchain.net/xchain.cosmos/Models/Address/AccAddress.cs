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

namespace Xchain.net.xchain.cosmos.Models.Address
{

    public class AccAddressJsonConvert : JsonConverter<AccAddress>
    {
        public override AccAddress Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return AccAddress.FromBech32(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, AccAddress value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToBech32());
        }
    }

    [JsonConverter(typeof(AccAddressJsonConvert))]
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
