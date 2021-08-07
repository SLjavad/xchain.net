using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XchainDotnet.Thorchain.Test.Fixture
{
    public class SdkFixture
    {
        public ThorchainClient Client { get; set; }
        public SdkFixture()
        {
            var phrase = "rural bright ball negative already grass good grant nation screen model pizza";
            Client = new ThorchainClient(phrase, null, null, XchainDotnet.Client.Models.Network.testnet);
        }
    }
}
