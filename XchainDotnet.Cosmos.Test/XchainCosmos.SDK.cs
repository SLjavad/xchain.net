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
using XchainDotnet.Cosmos.Models;
using XchainDotnet.Cosmos.Models.Account;
using XchainDotnet.Cosmos.Models.Address;
using XchainDotnet.Cosmos.Models.Common;
using XchainDotnet.Cosmos.Models.Message;
using XchainDotnet.Cosmos.Models.Message.Base;
using XchainDotnet.Cosmos.Models.Tx;
using XchainDotnet.Cosmos.Test.Fixture;
using XchainDotnet.Cosmos.Utils.JsonConverters;
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
        BroadcastTxParams txPost;
        private Mock<HttpMessageHandler> MockHttpClient(Dictionary<Expression<Func<HttpRequestMessage,bool>> ,
            HttpResponseMessage> mocks)
        {
            var mock = new Mock<HttpMessageHandler>();
            foreach (var item in mocks)
            {
                mock.Protected()
                    .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is(item.Key), ItExpr.IsAny<CancellationToken>())
                    .Callback<HttpRequestMessage, CancellationToken>(
                        (httpRequestMessage, cancellationToken) =>
                        {
                            if (httpRequestMessage.Method == HttpMethod.Post)
                            {
                                txPost = httpRequestMessage.Content
                                    .ReadFromJsonAsync<BroadcastTxParams>()
                                    .GetAwaiter()
                                    .GetResult();
                            }
                        })
                    .ReturnsAsync(item.Value);
            }
            return mock;
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

            GlobalHttpClient.HttpClient = new HttpClient(httpClient.Object);

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

            GlobalHttpClient.HttpClient = new HttpClient(httpClient.Object);

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

            GlobalHttpClient.HttpClient = new HttpClient(httpClient.Object);

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

            GlobalHttpClient.HttpClient = new HttpClient(httpClient.Object);

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
                            Denom = "thor"
                        }
                    }
                }
            });

            var mocks = new Dictionary<Expression<Func<HttpRequestMessage, bool>>, HttpResponseMessage>
            {
                {
                    x =>
                        x.Method == HttpMethod.Get
                        && x.RequestUri.OriginalString == $"{sdkClientFixture.ThorTestnetClient.server}/auth/accounts/{thorTestnetAddress}",
                    new HttpResponseMessage
                    {
                        Content = new StringContent(tempres)
                    }
                }
            };

            mocks.Add(x =>
                       x.Method == HttpMethod.Post
                       && x.RequestUri.OriginalString == $"{sdkClientFixture.ThorTestnetClient.server}/txs"
                       , new HttpResponseMessage
                       {
                           Content = new StringContent(expectedTxsPostResult)
                       });



            var httpClient = MockHttpClient(mocks);

            GlobalHttpClient.HttpClient = new HttpClient(httpClient.Object);

            var res = await sdkClientFixture.ThorTestnetClient.Transfer(new TransferParams
            {
                Amount = 10000,
                Asset = "thor",
                PrivKey = sdkClientFixture.ThorTestnetClient.GetPrivKeyFromMnemonic(thorPhrase),
                From = thorTestnetAddress,
                To = "tthor19kacmmyuf2ysyvq3t9nrl9495l5cvktj5c4eh4",
                Memo = "transfer"
            });
            var broadcastTxParams = new BroadcastTxParams()
            {
                Tx = new StdTx
                {
                    Memo = "transfer",
                    Msg = new List<Msg>
                    {
                        new AminoWrapper<MsgSend>
                        {
                            Type = "thorchain/MsgSend",
                            Value = new MsgSend
                            {
                                Amount = new List<Coin>
                                {
                                    new Coin
                                    {
                                        Amount = 10000,
                                        Denom = "thor"
                                    }
                                },
                                FromAddress = AccAddress.FromBech32(thorTestnetAddress),
                                ToAddress = AccAddress.FromBech32("tthor19kacmmyuf2ysyvq3t9nrl9495l5cvktj5c4eh4")
                            }
                        }
                    }
                }
            };

            var serialized = JsonSerializer.Serialize(broadcastTxParams, new JsonSerializerOptions
            {
                Converters =
                {
                    new MsgSendNumToStringConverter()
                }
            });
            //httpClient.Protected().Verify("SendAsync", Times.AtLeastOnce(), ItExpr.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post 
            //    && x.Content.ReadAsStringAsync().Result != null),
            //    ItExpr.IsAny<CancellationToken>());
            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(res));
            Assert.Single(txPost.Tx.Msg);
            Assert.Equal("transfer", txPost.Tx.Memo);
            Assert.Equal(thorTestnetAddress, ((AminoWrapper<MsgSend>)txPost.Tx.Msg[0]).Value.FromAddress.ToBech32());
            Assert.Equal("tthor19kacmmyuf2ysyvq3t9nrl9495l5cvktj5c4eh4", ((AminoWrapper<MsgSend>)txPost.Tx.Msg[0]).Value.FromAddress.ToBech32());
            Assert.Equal(10000, ((AminoWrapper<MsgSend>)txPost.Tx.Msg[0]).Value.Amount[0].Amount);
            Assert.Equal("thor", ((AminoWrapper<MsgSend>)txPost.Tx.Msg[0]).Value.Amount[0].Denom);
            Assert.Equal("thorchain/MsgSend", ((AminoWrapper<MsgSend>)txPost.Tx.Msg[0]).Type);
        }

        [Fact]
        public async Task Get_Transaction_Data()
        {
            sdkClientFixture.ThorTestnetClient.SetPrefix();
            var hash = "19BFC1E8EBB10AA1EC6B82E380C6F5FD349D367737EA8D55ADB4A24F0F7D1066";
            var tempres = JsonSerializer.Serialize(new TxResponse
            {
                Data = "0A090A076465706F736974",
                GasUsed = "148996",
                GasWanted = "5000000000000000",
                Height = 1047,
                RawLog = "transaction logs",
                TimeStamp = "2020-09-25T06:09:15Z",
                TxHash = "19BFC1E8EBB10AA1EC6B82E380C6F5FD349D367737EA8D55ADB4A24F0F7D1066",
                Tx = new RawTxResponse
                {
                    Body = new Body
                    {
                        Messages = new List<Msg>
                        {
                            new AminoWrapper<MsgSend>
                            {
                                Type = "thorchain/MsgSend",
                                Value = new MsgSend
                                {
                                    Amount = new List<Coin>
                                    {
                                        new Coin
                                        {
                                            Amount = 1000000,
                                            Denom = "thor"
                                        }
                                    },
                                    FromAddress = AccAddress.FromBech32("thor19kacmmyuf2ysyvq3t9nrl9495l5cvktjs0yfws"),
                                    ToAddress = AccAddress.FromBech32("thor19kacmmyuf2ysyvq3t9nrl9495l5cvktjs0yfws")
                                }
                            }
                        }
                    }
                }
            });
            var mocks = new Dictionary<Expression<Func<HttpRequestMessage, bool>>, HttpResponseMessage>
            {
                {
                    x =>
                        x.Method == HttpMethod.Get
                        && x.RequestUri.OriginalString == $"{sdkClientFixture.ThorTestnetClient.server}/txs/{hash}",
                    new HttpResponseMessage
                    {
                        Content = new StringContent(tempres)
                    }
                }
            };

            var httpMock = MockHttpClient(mocks);
            GlobalHttpClient.HttpClient = new HttpClient(httpMock.Object);

            var tx = await sdkClientFixture.ThorTestnetClient.TxHashGet("19BFC1E8EBB10AA1EC6B82E380C6F5FD349D367737EA8D55ADB4A24F0F7D1066");
            var txString = JsonSerializer.Serialize(tx);
            Assert.Equal(tempres, txString);
        }

    }
}
