using System.Collections.Generic;
using System.Dynamic;
using System.Text.Json.Serialization;
using XchainDotnet.Cosmos.Models.Address;
using XchainDotnet.Cosmos.Models.Crypto;
using XchainDotnet.Cosmos.Utils.JsonConverters;

namespace XchainDotnet.Cosmos.Models.Account
{
    /// <summary>
    /// Base Account type
    /// </summary>
    public class BaseAccount
    {
        /// <summary>
        /// Account Address
        /// </summary>
        [JsonPropertyName("address")]
        public AccAddress Address { get; set; }
        /// <summary>
        /// Account public key
        /// </summary>
        [JsonPropertyName("public_key")]
        [JsonConverter(typeof(PublicKeyJsonConverter))]
        public IPublicKey PublicKey { get; set; }
        /// <summary>
        /// Account Coins
        /// </summary>
        [JsonPropertyName("coins")]
        public List<Coin> Coins { get; set; }
        /// <summary>
        /// Account number
        /// </summary>
        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; } = "0";
        /// <summary>
        /// Account Sequence
        /// </summary>
        [JsonPropertyName("sequence")]
        public string Sequence { get; set; } = "0";

        public static BaseAccount FromJson(dynamic value)
        {
            var acc = new BaseAccount
            {
                AccountNumber = value.account_number,
                Address = IsPropertyExist(value, "address") && value.address != null ? AccAddress.FromBech32(value.address) : null,
                Coins = IsPropertyExist(value, "coins") && value.coins != null ? value.coins : null,
                PublicKey = IsPropertyExist(value, "public_key") && value.public_key != null ? value.public_key : null,
                Sequence = IsPropertyExist(value, "sequence") && value.sequence != null ? value.sequence : 0
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


