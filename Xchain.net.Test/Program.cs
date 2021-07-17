using System;
using System.Text;
using Xchain.net.xchain.cosmos.Models.Crypto.RIPEMD160;
using Xchain.net.xchain.cosmos.SDK;
using Xchain.net.xchain.thorchain;

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

        private static void TestgetTransactionData()
        {
            string mnemonic = "rural bright ball negative already grass good grant nation screen model pizza";
            var thorClient = new ThorchainClient(mnemonic, null, null, xchain.client.Models.Network.testnet);
            var res = thorClient.GetTranasctionData("9D2B0AB362A5BF8689F0A984AB9B76129B83917D13638203FC7B03D6D97971BC");
        }

        static void Main(string[] args)
        {
            TestgetTransactionData();
            Console.ReadKey();
        }
    }
}
