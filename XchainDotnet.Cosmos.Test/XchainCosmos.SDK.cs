using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using XchainDotnet.Client;
using XchainDotnet.Cosmos.Models.Account;
using XchainDotnet.Cosmos.Models.Common;
using XchainDotnet.Cosmos.Models.Tx;
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

        private HttpClient MockHttpClient(Dictionary<Expression<Func<HttpRequestMessage,bool>> ,
            HttpResponseMessage> mocks,
            Expression<Func<HttpRequestMessage, bool>> verifiers = null)
        {
            var mock = new Mock<HttpMessageHandler>();
            foreach (var item in mocks)
            {
                mock.Protected()
                    .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is(item.Key), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(item.Value);
            }
            if (verifiers != null)
            {
                mock.Protected().Verify("SendAsync", Times.AtLeastOnce(), ItExpr.Is(verifiers));
            }
            var httpClient = new HttpClient(mock.Object);
            return httpClient;
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

        [Fact]
        public async Task GetBalance()
        {
            var tempres = JsonSerializer.Serialize(new CommonResponse<List<Models.Coin>>
            {
                Height = "0",
                Result = new List<Models.Coin>()
            });

            var mocks = new Dictionary<Expression<Func<HttpRequestMessage, bool>>, HttpResponseMessage>
            { {
                    x => x.Method == HttpMethod.Get
                          && x.RequestUri.OriginalString == $"{sdkClientFixture.CosmosMainnetClient.server}/bank/balances/{cosmosAddress}",
                    new HttpResponseMessage
                    {
                        Content = new StringContent(tempres)
                    }
                }
            };

            var httpClient = MockHttpClient(mocks);

            GlobalHttpClient.HttpClient = httpClient;

            var res = await sdkClientFixture.CosmosMainnetClient.GetBalance(cosmosAddress);
            Assert.True(res.Count == 0);

            // =========================================

            tempres = JsonSerializer.Serialize(new CommonResponse<List<Models.Coin>>
            {
                Height = "0",
                Result = new List<Models.Coin>()
                {
                    new Models.Coin
                    {
                        Amount = 75000000,
                        Denom = "umuon"
                    }
                }
            });
            mocks = new Dictionary<Expression<Func<HttpRequestMessage, bool>>, HttpResponseMessage>
            {
                {
                    x =>
                        x.Method == HttpMethod.Get
                        && x.RequestUri.OriginalString == $"{sdkClientFixture.CosmosTestnetClient.server}/bank/balances/cosmos1gehrq0pr5d79q8nxnaenvqh09g56jafm82thjv",
                    new HttpResponseMessage
                    {
                        Content = new StringContent(tempres)
                    }
                }
            };

            httpClient = MockHttpClient(mocks);

            GlobalHttpClient.HttpClient = httpClient;

            res = await sdkClientFixture.CosmosTestnetClient.GetBalance("cosmos1gehrq0pr5d79q8nxnaenvqh09g56jafm82thjv");
            Assert.Equal(75000000, res[0].Amount);
            Assert.Equal("umuon", res[0].Denom);

            // ============================================

            tempres = JsonSerializer.Serialize(new CommonResponse<List<Models.Coin>>
            {
                Height = "0",
                Result = new List<Models.Coin>()
                {
                    new Models.Coin
                    {
                        Amount = 100,
                        Denom = "thor"
                    }
                }
            });
            mocks = new Dictionary<Expression<Func<HttpRequestMessage, bool>>, HttpResponseMessage>
            {
                {
                    x =>
                        x.Method == HttpMethod.Get
                        && x.RequestUri.OriginalString == $"{sdkClientFixture.ThorMainnetClient.server}/bank/balances/{thorMainnetAddress}",
                    new HttpResponseMessage
                    {
                        Content = new StringContent(tempres)
                    }
                }
            };

            httpClient = MockHttpClient(mocks);

            GlobalHttpClient.HttpClient = httpClient;

            res = await sdkClientFixture.ThorMainnetClient.GetBalance(thorMainnetAddress);
            Assert.Single(res);
            Assert.Equal("thor", res[0].Denom);
            Assert.Equal(100, res[0].Amount);

            // ==============================================

            tempres = JsonSerializer.Serialize(new CommonResponse<List<Models.Coin>>
            {
                Height = "0",
                Result = new List<Models.Coin>()
            });

            mocks = new Dictionary<Expression<Func<HttpRequestMessage, bool>>, HttpResponseMessage>
            {
                {
                    x =>
                        x.Method == HttpMethod.Get
                        && x.RequestUri.OriginalString == $"{sdkClientFixture.ThorTestnetClient.server}/bank/balances/{thorTestnetAddress}",
                    new HttpResponseMessage
                    {
                        Content = new StringContent(tempres)
                    }
                }
            };

            httpClient = MockHttpClient(mocks);

            GlobalHttpClient.HttpClient = httpClient;

            res = await sdkClientFixture.ThorTestnetClient.GetBalance(thorTestnetAddress);
            Assert.True(res.Count == 0);
        }

        [Fact]
        public async Task Transfer()
        {
            var expected = new BroadcastTxCommitResult
            {
                CheckTxResult = new CheckTxResult(),
                DeliverTxResult = new DeliverTxResult(),
                TxHash = "EA2FAC9E82290DCB9B1374B4C95D7C4DD8B9614A96FACD38031865EB1DBAE24D",
                Height = 0
            };
            var expectedTxsPostResult = JsonSerializer.Serialize(expected);

            var tempres = JsonSerializer.Serialize(new CommonResponse<BaseAccount>
            {
                Height = "0",
                Result = new BaseAccount
                {
                    AccountNumber = "0",
                    Sequence = "0",
                    Coins = new List<Models.Coin>
                    {
                        new Models.Coin
                        {
                            Amount = 21000,
                            Denom = "muon"
                        }
                    }
                }
            });

            var mocks = new Dictionary<Expression<Func<HttpRequestMessage, bool>>, HttpResponseMessage>
            {
                {
                    x =>
                        x.Method == HttpMethod.Get
                        && x.RequestUri.OriginalString == $"{sdkClientFixture.CosmosTestnetClient.server}/auth/accounts/{cosmosAddress}",
                    new HttpResponseMessage
                    {
                        Content = new StringContent(tempres)
                    }
                }
            };

            mocks.Add(x =>
                       x.Method == HttpMethod.Post
                       && x.RequestUri.OriginalString == $"{sdkClientFixture.CosmosTestnetClient.server}/txs"
                       , new HttpResponseMessage
                       {
                           Content = new StringContent(expectedTxsPostResult)
                       });

            var httpClient = MockHttpClient(mocks);

            GlobalHttpClient.HttpClient = httpClient;

            var res = await sdkClientFixture.CosmosTestnetClient.Transfer(new TransferParams
            {
                Amount = 10000,
                Asset = "muon",
                PrivKey = sdkClientFixture.CosmosTestnetClient.GetPrivKeyFromMnemonic(cosmosPhrase),
                From = cosmosAddress,
                To = "cosmos1gehrq0pr5d79q8nxnaenvqh09g56jafm82thjv",
                Memo = "transfer"
            });
            Assert.Equal(expected, res);
        }

    }
}
