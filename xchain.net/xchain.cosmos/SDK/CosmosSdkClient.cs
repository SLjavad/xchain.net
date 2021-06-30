using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchain.net.xchain.cosmos.Models.Address;
using Xchain.net.xchain.cosmos.Models.Crypto;

namespace Xchain.net.xchain.cosmos.SDK
{
    public class CosmosSdkClient
    {
        private readonly string server;
        private readonly string chainId;
        private readonly string prefix;
        private readonly string derivePath;

        private const string BASE_PATH = "https://api.cosmos.network";

        public CosmosSdkClient(string server , string chainId , string prefix = "cosmos" , string derivePath = "44'/118'/0'/0/0")
        {
            this.server = server;
            this.chainId = chainId;
            this.prefix = prefix;
            this.derivePath = derivePath;
        }

        public void SetPrefix()
        {
            BaseAddress.SetBech32Prefix(
                this.prefix,
                this.prefix + "pub",
                this.prefix + "valoper",
                this.prefix + "valoperpub",
                this.prefix + "valcons",
                this.prefix + "valconspub");
        }

        public string GetAddressFromPrivKey(IPrivateKey privateKey)
        {
            this.SetPrefix();
            return AccAddress.FromPublicKey(privateKey.GetPublicKey()).ToBech32();
        }
    }
}
