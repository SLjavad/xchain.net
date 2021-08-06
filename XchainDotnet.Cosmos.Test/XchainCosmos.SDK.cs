using System;
using XchainDotnet.Cosmos.Test.Fixture;
using Xunit;

namespace XchainDotnet.Cosmos.Test
{
    public class XchainCosmosTest : IClassFixture<SdkClientFixture>
    {
        private readonly SdkClientFixture sdkClientFixture;
        private string cosmosPhrase = "foster blouse cattle fiction deputy social brown toast various sock awkward print";
        private string cosmosAddress = "cosmos16mzuy68a9xzqpsp88dt4f2tl0d49drhepn68fg";
        private string thorPhrase = "rural bright ball negative already grass good grant nation screen model pizza";
        private string thorMainnetAddress = "thor19kacmmyuf2ysyvq3t9nrl9495l5cvktjs0yfws";
        private string thorTestnetAddress = "tthor19kacmmyuf2ysyvq3t9nrl9495l5cvktj5c4eh4";

        public XchainCosmosTest(SdkClientFixture sdkClientFixture)
        {
            this.sdkClientFixture = sdkClientFixture;
        }

        private void MockHttpClient()
        {

        }

        [Fact]
        public void Get_Address_from_PrivKey()
        {
            var privKey = sdkClientFixture.CosmosMainnetClient.GetPrivKeyFromMnemonic(cosmosPhrase);
            Assert.Equal(cosmosAddress, sdkClientFixture.CosmosMainnetClient.GetAddressFromPrivKey(privKey));

            privKey = sdkClientFixture.CosmosTestnetClient.GetPrivKeyFromMnemonic(cosmosPhrase);
            Assert.Equal(cosmosAddress, sdkClientFixture.CosmosTestnetClient.GetAddressFromPrivKey(privKey));

            privKey = sdkClientFixture.ThorMainnetClient.GetPrivKeyFromMnemonic(thorPhrase);
            Assert.Equal(thorMainnetAddress, sdkClientFixture.ThorMainnetClient.GetAddressFromPrivKey(privKey));

            privKey = sdkClientFixture.ThorTestnetClient.GetPrivKeyFromMnemonic(thorPhrase);
            Assert.Equal(thorTestnetAddress, sdkClientFixture.ThorTestnetClient.GetAddressFromPrivKey(privKey));
        }

        [Fact]
        public void CheckAddress()
        {
            Assert.True(sdkClientFixture.CosmosMainnetClient.CheckAddress(cosmosAddress));
            Assert.True(sdkClientFixture.CosmosTestnetClient.CheckAddress(cosmosAddress));
            Assert.False(sdkClientFixture.CosmosMainnetClient.CheckAddress("thor19kacmmyuf2ysyvq3t9nrl9495l5cvktjs0yfws"));
            Assert.False(sdkClientFixture.CosmosTestnetClient.CheckAddress("tthor19kacmmyuf2ysyvq3t9nrl9495l5cvktj5c4eh4"));

            Assert.True(sdkClientFixture.ThorMainnetClient.CheckAddress("thor19kacmmyuf2ysyvq3t9nrl9495l5cvktjs0yfws"));
            Assert.True(sdkClientFixture.ThorTestnetClient.CheckAddress("tthor19kacmmyuf2ysyvq3t9nrl9495l5cvktj5c4eh4"));
            Assert.False(sdkClientFixture.ThorMainnetClient.CheckAddress(cosmosAddress));
            Assert.False(sdkClientFixture.ThorTestnetClient.CheckAddress(cosmosAddress));
            Assert.False(sdkClientFixture.ThorMainnetClient.CheckAddress("tthor19kacmmyuf2ysyvq3t9nrl9495l5cvktj5c4eh4"));
            Assert.False(sdkClientFixture.ThorTestnetClient.CheckAddress("thor19kacmmyuf2ysyvq3t9nrl9495l5cvktjs0yfws"));
        }
    }
}
