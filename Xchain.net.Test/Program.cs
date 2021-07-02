using System;
using System.Text;
using Xchain.net.xchain.cosmos.Models.Crypto.RIPEMD160;
using Xchain.net.xchain.cosmos.SDK;

namespace Xchain.net.Test
{
    class Program
    {

        private static void RIPEMD160()
        {
            var item = RIPEMD160Crypto.Create();
            var enc = item.ComputeHash(Encoding.UTF8.GetBytes("asd"));

            string hex = BitConverter.ToString(enc).Replace("-", " ");

            Console.WriteLine(hex);
            Console.ReadKey();
        }

        private static void testPrivKey()
        {
            string mnemonic = "rural bright ball negative already grass good grant nation screen model pizza";
            var client = new CosmosSdkClient("test", "thorchain", "tthor", "44'/931'/0'/0/0");
            var key = client.GetPrivKeyFromMnemonic(mnemonic);
            string hex = BitConverter.ToString(key.ToBuffer()).Replace("-", " ");
        }

        static void Main(string[] args)
        {
            testPrivKey();
        }
    }
}
