using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XchainDotnet.Cosmos.SDK;

namespace XchainDotnet.Cosmos.Test.Fixture
{
    public class SdkClientFixture
    {
        public CosmosSdkClient CosmosMainnetClient { get; private set; }
        public CosmosSdkClient CosmosTestnetClient { get; private set; }
        public CosmosSdkClient ThorMainnetClient { get; private set; }
        public CosmosSdkClient ThorTestnetClient { get; private set; }

        public SdkClientFixture()
        {
            CosmosMainnetClient = new CosmosSdkClient("https://api.cosmos.network", "cosmoshub-3", "cosmos");
            CosmosTestnetClient = new CosmosSdkClient("http://lcd.gaia.bigdipper.live:1317", "gaia-3a", "cosmos");
            ThorMainnetClient = new CosmosSdkClient("http://104.248.96.152:1317", "thorchain", "thor", "44'/931'/0'/0/0");
            ThorTestnetClient = new CosmosSdkClient("http://13.238.212.224:1317", "thorchain", "tthor", "44'/931'/0'/0/0");
        }
    }
}
