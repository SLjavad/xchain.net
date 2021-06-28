using System;
using System.Text;
using Xchain.net.xchain.cosmos.Models.Crypto.RIPEMD160;

namespace Xchain.net.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var item = RIPEMD160Crypto.Create();
            var enc = item.ComputeHash(Encoding.UTF8.GetBytes("asd"));

            string hex = BitConverter.ToString(enc).Replace("-", " ");

            Console.WriteLine(hex);
            Console.ReadKey();
        }
    }
}
