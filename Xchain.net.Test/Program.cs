using System;
using System.Text;
using System.Threading.Tasks;
using XchainDotnet.Client.Models;
using XchainDotnet.Cosmos.Models.Crypto.RIPEMD160;
using XchainDotnet.Cosmos.SDK;
using XchainDotnet.Thorchain;
using XchainDotnet.Thorchain.Models;

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

        private static async Task TestgetTransactionData()
        {
            string mnemonic = "rural bright ball negative already grass good grant nation screen model pizza";
            var thorClient = new ThorchainClient(mnemonic, null, null, Network.testnet);
            var res = await thorClient.GetTranasctionData("DFEC1DC8ED2F7D5C2578E31E8D58B53688080097EF6806766D3074851E6C25A9");
            Console.WriteLine(res);
        }

        private static async Task TestDeposit()
        {
            var send_amount = 10000;
            var memo = "swap:BNB.BNB:tbnb1ftzhmpzr4t8ta3etu4x7nwujf9jqckp3th2lh0";
            string mnemonic = "rural bright ball negative already grass good grant nation screen model pizza";
            var client = new ThorchainClient(mnemonic, null, null, Network.testnet);
            var result = await client.Deposit(new DepositParam
            {
                Amount = send_amount,
                Memo = memo
            });
            Console.WriteLine(result);
        }

        private static async Task Transfer()
        {
            var send_amount = 10000;
            var memo = "transfer";
            string mnemonic = "rural bright ball negative already grass good grant nation screen model pizza";
            var to_address = "tthor1pttyuys2muhj674xpr9vutsqcxj9hepy4ddueq";
            var client = new ThorchainClient(mnemonic, null, null, Network.testnet);
            var result = await client.Transfer(new TxParams
            {
                Asset = new AssetRune(),
                Amount = send_amount,
                Memo = memo,
                Recipient = to_address
            });
            Console.WriteLine(result);
        }

        private static async Task TestGetTransactions()
        {
            var send_amount = 10000;
            var memo = "transfer";
            string mnemonic = "rural bright ball negative already grass good grant nation screen model pizza";
            var address = "tthor13gym97tmw3axj3hpewdggy2cr288d3qffr8skg";
            var client = new ThorchainClient(mnemonic, null, null, Network.testnet);
            var result = await client.GetTransactions(new TxHistoryParamFilter
            {
                Address = address,
                Limit = 10
            });
            Console.WriteLine(result);
        }

        static void Main(string[] args)
        {
            TestDeposit();
            Console.ReadKey();
        }
    }
}
