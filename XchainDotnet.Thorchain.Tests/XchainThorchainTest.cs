using System;
using System.Threading.Tasks;
using XchainDotnet.Client.Models;
using XchainDotnet.Cosmos.SDK;
using XchainDotnet.Thorchain.Exceptions;
using XchainDotnet.Thorchain.Test.Fixture;
using Xunit;

namespace XchainDotnet.Thorchain.Tests
{
    public class XchainThorchainTest : IClassFixture<SdkFixture>
    {
        private readonly SdkFixture sdkFixture;
        private string phrase = "rural bright ball negative already grass good grant nation screen model pizza";
        private string mainnetAddress = "thor19kacmmyuf2ysyvq3t9nrl9495l5cvktjs0yfws";
        private string testnetAddress = "tthor19kacmmyuf2ysyvq3t9nrl9495l5cvktj5c4eh4";

        public XchainThorchainTest(SdkFixture sdkFixture)
        {
            this.sdkFixture = sdkFixture;
        }

        [Fact]
        public void Should_start_with_empty_wallet()
        {
            var thorClientEmptyMain = new ThorchainClient(phrase, null, null, Client.Models.Network.mainnet);
            var addressMain = thorClientEmptyMain.Address;
            Assert.Equal(mainnetAddress, addressMain);

            var thorClientEmptyTest = new ThorchainClient(phrase, null, null, Client.Models.Network.testnet);
            var addressTest = thorClientEmptyTest.Address;
            Assert.Equal(testnetAddress, addressTest);
        }

        [Fact]
        public void Throw_error_when_invalid_phrase()
        {
            Assert.Throws<PhraseNotValidException>(() => new ThorchainClient("invalid phrase", null, null, Client.Models.Network.mainnet));
            Assert.Throws<PhraseNotValidException>(() => new ThorchainClient("invalid phrase", null, null, Client.Models.Network.testnet));
        }

        [Fact]
        public void Should_have_right_address()
        {
            Assert.Equal(testnetAddress, sdkFixture.Client.Address);

            sdkFixture.Client.Network = Network.mainnet;
            Assert.Equal(mainnetAddress, sdkFixture.Client.Address);
        }

        [Fact]
        public void Should_allow_to_get_the_CosmosSdkClient()
        {
            Assert.IsType<CosmosSdkClient>(sdkFixture.Client.ThorClient);
        }

        [Fact]
        public void Should_Update_Net()
        {
            var client = new ThorchainClient(phrase, null, null, Client.Models.Network.mainnet);
            client.Network = Network.testnet;
            Assert.Equal(Network.testnet, client.Network);

            var address = client.Address;
            Assert.Equal(testnetAddress, address);
        }

        [Fact]
        public void Should_Init_Should_have_right_prefix()
        {
            Assert.True(sdkFixture.Client.ValidateAddress(sdkFixture.Client.Address));

            sdkFixture.Client.Network = Network.mainnet;
            Assert.True(sdkFixture.Client.ValidateAddress(sdkFixture.Client.Address));
        }

        [Fact]
        public void Should_have_right_client_url()
        {
            sdkFixture.Client.ClientUrl = new Models.ClientUrl
            {
                Mainnet = new Models.NodeUrl
                {
                    Node = "new mainnet client",
                    RPC = "new mainnet client"
                },
                Testnet = new Models.NodeUrl
                {
                    RPC = "new testnet client",
                    Node = "new testnet client"
                }
            };

            sdkFixture.Client.Network = Network.mainnet;
            Assert.Equal("new mainnet client", sdkFixture.Client.ClientUrl.GetByNetwork(sdkFixture.Client.Network).Node);

            sdkFixture.Client.Network = Network.testnet;
            Assert.Equal("new testnet client", sdkFixture.Client.ClientUrl.GetByNetwork(sdkFixture.Client.Network).Node);
        }

        [Fact]
        public void Has_No_Balance()
        {
            sdkFixture.Client.Network = Network.mainnet;


        }
    }
}
