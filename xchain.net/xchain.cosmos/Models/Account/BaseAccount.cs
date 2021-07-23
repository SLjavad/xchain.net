using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xchain.net.xchain.cosmos.Models.Address;
using Xchain.net.xchain.cosmos.Models.Crypto;

namespace Xchain.net.xchain.cosmos.Models.Account
{
    public class BaseAccount
    {
        [JsonPropertyName("address")]
        public AccAddress Address { get; set; }
        [JsonPropertyName("public_key")]
        [JsonConverter(typeof(PublicKeyJsonConverter))]
        public IPublicKey PublicKey { get; set; }
        [JsonPropertyName("coins")]
        public List<Coin> Coins { get; set; }
        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; }
        [JsonPropertyName("sequence")]
        public string Sequence { get; set; }

        public static BaseAccount FromJson(dynamic value)
        {
            var acc = new BaseAccount
            {
                AccountNumber = value.account_number,
                Address = (IsPropertyExist(value,"address") && value.address != null) ? AccAddress.FromBech32(value.address) : null,
                Coins = (IsPropertyExist(value, "coins") && value.coins != null) ? value.coins : null,
                PublicKey = (IsPropertyExist(value, "public_key") && value.public_key != null) ? value.public_key : null,
                Sequence = (IsPropertyExist(value, "sequence") && value.sequence != null) ? value.sequence : 0
            };
            return acc;
        }

        private static bool IsPropertyExist(dynamic settings, string name)
        {
            if (settings is ExpandoObject)
                return ((IDictionary<string, object>)settings).ContainsKey(name);

            return settings.GetType().GetProperty(name) != null;
        }
    }
}


