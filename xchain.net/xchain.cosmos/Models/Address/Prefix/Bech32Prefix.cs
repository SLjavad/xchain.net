using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchain.net.xchain.cosmos.Models.Address.Prefix
{
    public class Prefix
    {
        public const string Cosmos = "cosmos";
        public const string Public = "pub";
        public const string Account = "acc";
        public const string Validator = "val";
        public const string Operator = "oper";
        public const string Consensus = "cons";
    }

    public static class Bech32Prefix
    {
        public static string AccAddr { get; set; } = Prefix.Cosmos;
        public static string AccPub { get; set; } = Prefix.Cosmos + Prefix.Public;
        public static string ValAddr { get; set; } = Prefix.Cosmos + Prefix.Validator + Prefix.Operator;
        public static string ValPub { get; set; } = Prefix.Cosmos + Prefix.Validator + Prefix.Operator + Prefix.Public;
        public static string ConsAddr { get; set; } = Prefix.Cosmos + Prefix.Validator + Prefix.Consensus;
        public static string ConsPub  { get; set; } = Prefix.Cosmos + Prefix.Validator + Prefix.Consensus + Prefix.Public;
    }
}
