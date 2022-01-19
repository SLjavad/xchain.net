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
using XchainDotnet.Client.Models;
using XchainDotnet.Cosmos.Models;
using XchainDotnet.Cosmos.Models.Account;
using XchainDotnet.Cosmos.Models.Address;
using XchainDotnet.Cosmos.Models.Common;
using XchainDotnet.Cosmos.Models.Message;
using XchainDotnet.Cosmos.Models.Message.Base;
using XchainDotnet.Cosmos.Models.RPC;
using XchainDotnet.Cosmos.Models.Tx;
using XchainDotnet.Cosmos.SDK;
using XchainDotnet.Thorchain.Exceptions;
using XchainDotnet.Thorchain.Models;
using XchainDotnet.Thorchain.Test.Fixture;
using Xunit;

namespace XchainDotnet.Thorchain.Tests
{
    public class XchainThorchainTest : IClassFixture<SdkFixture>
    {
        private readonly SdkFixture sdkFixture;
        private readonly ThorchainClient thorClient;
        private readonly ThorchainClient thorMainClient;
        private string phrase = "rural bright ball negative already grass good grant nation screen model pizza";
        private string mainnet_address_path0 = "thor19kacmmyuf2ysyvq3t9nrl9495l5cvktjs0yfws";
        private string mainnet_address_path1 = "thor1hrf34g3lxwvpk7gjte0xvahf3txnq8ecgaf4nc";
        private string testnet_address_path0 = "tthor19kacmmyuf2ysyvq3t9nrl9495l5cvktj5c4eh4";
        private string testnet_address_path1 = "tthor1hrf34g3lxwvpk7gjte0xvahf3txnq8ecv2c92a";

        public XchainThorchainTest(SdkFixture sdkFixture)
        {
            var phrase = "rural bright ball negative already grass good grant nation screen model pizza";
            this.thorClient = new ThorchainClient(phrase, null, null,null, XchainDotnet.Client.Models.Network.testnet);
            this.thorMainClient = new ThorchainClient(phrase, null, null,null, XchainDotnet.Client.Models.Network.mainnet);
        }

        BroadcastTxParams txsPost;
        BroadcastTxParams depositPost;
        private Mock<HttpMessageHandler> MockHttpClient(Dictionary<Expression<Func<HttpRequestMessage, bool>>,
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
                            if (httpRequestMessage.Method == HttpMethod.Post 
                            && httpRequestMessage.RequestUri.OriginalString == $"{this.thorClient.ClientUrl.GetByNetwork(this.thorClient.Network.Value).Node}/txs")
                            {
                                txsPost = httpRequestMessage.Content
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
        public void Should_start_with_empty_wallet()
        {
            var thorClientEmptyMain = new ThorchainClient(phrase, null, null,null, Client.Models.Network.mainnet);
            var addressMain = thorClientEmptyMain.GetAddress();
            Assert.Equal(mainnet_address_path0, addressMain);

            var thorClientEmptyTest = new ThorchainClient(phrase, null, null,null, Client.Models.Network.testnet);
            var addressTest = thorClientEmptyTest.GetAddress();
            Assert.Equal(testnet_address_path0, addressTest);
        }

        [Fact]
        public void should_derive_address_accordingly_to_the_user_param()
        {
            var thorClientEmptyMain = new ThorchainClient(phrase, null, null, null, Client.Models.Network.mainnet);
            var addressMain = thorClientEmptyMain.GetAddress();
            Assert.Equal(mainnet_address_path1, addressMain);

            var viaSetPhraseAddr1 = thorClientEmptyMain.GetAddress(1);
            Assert.Equal(mainnet_address_path0, viaSetPhraseAddr1);

            var thorClientEmptyTest = new ThorchainClient(phrase, null, null, null, Client.Models.Network.testnet);
            var addressTest = thorClientEmptyTest.GetAddress();
            Assert.Equal(testnet_address_path0, addressTest);

            var viaSetPhraseAddr1Test = thorClientEmptyTest.GetAddress(1);
            Assert.Equal(testnet_address_path1, viaSetPhraseAddr1Test);

            thorClientEmptyMain = new ThorchainClient(phrase, null, null, null, Client.Models.Network.mainnet);
            var addressMain1 = thorClientEmptyMain.GetAddress(1);
            Assert.Equal(mainnet_address_path0, addressMain1);

            thorClientEmptyTest = new ThorchainClient(phrase, null, null, null, Client.Models.Network.testnet);
            var addressTest1 = thorClientEmptyTest.GetAddress(1);
            Assert.Equal(testnet_address_path1, addressTest1);
        }

        [Fact]
        public void Throw_error_when_invalid_phrase()
        {
            Assert.Throws<PhraseNotValidException>(() => new ThorchainClient("invalid phrase", null, null,null, Client.Models.Network.mainnet));
            Assert.Throws<PhraseNotValidException>(() => new ThorchainClient("invalid phrase", null, null,null, Client.Models.Network.testnet));
        }

        [Fact]
        public void Should_have_right_address()
        {
            Assert.Equal(testnet_address_path0, this.thorClient.GetAddress());

            this.thorClient.Network = Network.mainnet;
            Assert.Equal(mainnet_address_path0, this.thorClient.GetAddress());
        }

        [Fact]
        public void Should_allow_to_get_the_CosmosSdkClient()
        {
            Assert.IsType<CosmosSdkClient>(this.thorClient.CosmosClient);
        }

        [Fact]
        public void Should_Update_Net()
        {
            var client = new ThorchainClient(phrase, null, null,null, Client.Models.Network.mainnet);
            client.Network = Network.testnet;
            Assert.Equal(Network.testnet, client.Network);

            var address = client.GetAddress();
            Assert.Equal(testnet_address_path0, address);
        }

        [Fact]
        public void Should_Init_Should_have_right_prefix()
        {
            Assert.True(this.thorClient.ValidateAddress(this.thorClient.GetAddress()));

            this.thorClient.Network = Network.mainnet;
            Assert.True(this.thorClient.ValidateAddress(this.thorClient.GetAddress()));
        }

        [Fact]
        public void Should_return_valid_explorer_url()
        {
            Assert.Equal("https://viewblock.io/thorchain?network=testnet", this.thorClient.GetExplorerUrl());

            this.thorClient.Network = Network.mainnet;
            Assert.Equal("https://viewblock.io/thorchain", this.thorClient.GetExplorerUrl());
        }

        [Fact]
        public void Should_return_valid_explorer_address_url()
        {
            Assert.Equal("https://viewblock.io/thorchain/address/anotherTestAddressHere?network=testnet", this.thorClient.GetExplorerAddressUrl("anotherTestAddressHere"));

            this.thorClient.Network = Network.mainnet;
            Assert.Equal("https://viewblock.io/thorchain/address/testAddressHere", this.thorClient.GetExplorerAddressUrl("testAddressHere"));
        }

        [Fact]
        public void Should_return_valid_explorer_tx_url()
        {
            Assert.Equal("https://viewblock.io/thorchain/tx/anotherTestTxHere?network=testnet", this.thorClient.GetExplorerTxUrl("anotherTestTxHere"));

            this.thorClient.Network = Network.mainnet;
            Assert.Equal("https://viewblock.io/thorchain/tx/testTxHere", this.thorClient.GetExplorerTxUrl("testTxHere"));
        }

        [Fact]
        public void Should_have_right_client_url()
        {
            this.thorClient.ClientUrl = new Models.ClientUrl
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

            this.thorClient.Network = Network.mainnet;
            Assert.Equal("new mainnet client", this.thorClient.ClientUrl.GetByNetwork(this.thorClient.Network.Value).Node);

            this.thorClient.Network = Network.testnet;
            Assert.Equal("new testnet client", this.thorClient.ClientUrl.GetByNetwork(this.thorClient.Network.Value).Node);
        }

        [Fact]
        public async Task Has_No_Balance()
        {
            var tempres = JsonSerializer.Serialize(new CommonResponse<List<Coin>>
            {
                Height = "0",
                Result = new List<Coin>()
            });

            var mocks = new Dictionary<Expression<Func<HttpRequestMessage, bool>>, HttpResponseMessage>
            { {
                    x => x.Method == HttpMethod.Get
                          && x.RequestUri.OriginalString == $"{this.thorClient.ClientUrl.GetByNetwork(this.thorClient.Network.Value).Node}" +
                          $"/bank/balances/{testnet_address_path0}",
                    new HttpResponseMessage
                    {
                        Content = new StringContent(tempres)
                    }
                }
            };

            var httpClient = MockHttpClient(mocks);

            GlobalHttpClient.HttpClient = new HttpClient(httpClient.Object);

            var result = await this.thorClient.GetBalance();

            Assert.True(result.Count == 0);
        }

        [Fact]
        public async Task Has_Balance()
        {
            this.thorClient.Network = Network.mainnet;
            var tempres = JsonSerializer.Serialize(new CommonResponse<List<Coin>>
            {
                Height = "0",
                Result = new List<Coin>()
                {
                    new Coin
                    {
                        Amount = "100",
                        Denom = "rune"
                    }
                }
            });

            var mocks = new Dictionary<Expression<Func<HttpRequestMessage, bool>>, HttpResponseMessage>
            { {
                    x => x.Method == HttpMethod.Get
                          && x.RequestUri.OriginalString == $"{this.thorClient.ClientUrl.GetByNetwork(this.thorClient.Network.Value).Node}" +
                          $"/bank/balances/thor147jegk6e9sum7w3svy3hy4qme4h6dqdkgxhda5",
                    new HttpResponseMessage
                    {
                        Content = new StringContent(tempres)
                    }
                }
            };

            var httpClient = MockHttpClient(mocks);

            GlobalHttpClient.HttpClient = new HttpClient(httpClient.Object);

            var result = await this.thorClient.GetBalance("thor147jegk6e9sum7w3svy3hy4qme4h6dqdkgxhda5");

            Assert.True(result.Count == 1);
            Assert.IsType<AssetRune>(result[0].Asset);
            Assert.Equal(100, result[0].Amount);
        }

        [Fact]
        public async Task Has_an_empty_tx_history()
        {
            //this.thorClient.Network = Network.mainnet;

            var expected = new TxPage
            {
                Total = 0,
                Txs = new List<Tx>()
            };

            var tempres = JsonSerializer.Serialize(new RPCResponse<RPCTxSearchResult>
            {
                Id = -1,
                JsonRPC = "2.0",
                Result = new RPCTxSearchResult
                {
                    Txs = new List<RPCTxResult>(),
                    TotalCount = "0"
                }
            });

            var mocks = new Dictionary<Expression<Func<HttpRequestMessage, bool>>, HttpResponseMessage>
            { {
                    x => x.Method == HttpMethod.Get
                          && x.RequestUri.OriginalString.StartsWith($"{this.thorClient.ClientUrl.GetByNetwork(this.thorClient.Network.Value).RPC}" +
                          $"/tx_search"),
                    new HttpResponseMessage
                    {
                        Content = new StringContent(tempres)
                    }
                }
            };

            var httpClient = MockHttpClient(mocks);

            GlobalHttpClient.HttpClient = new HttpClient(httpClient.Object);

            var result = await this.thorClient.GetTransactions(new TxHistoryParamFilter
            {
                Address = "tthor13gym97tmw3axj3hpewdggy2cr288d3qffr8skg",
                Limit = 1
            });

            httpClient.Protected().Verify("SendAsync", Times.Exactly(2) , ItExpr.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Get
                          && x.RequestUri.OriginalString.StartsWith($"{this.thorClient.ClientUrl.GetByNetwork(this.thorClient.Network.Value).RPC}" +
                          $"/tx_search")), ItExpr.IsAny<CancellationToken>());

            Assert.Equal(expected.Total, result.Total);
            Assert.Equal(expected.Txs.Count, result.Txs.Count);
        }

        [Fact]
        public async Task Has_Tx_history()
        {
            var tempres = JsonSerializer.Serialize(new RPCResponse<RPCTxSearchResult>
            {
                Id = -1,
                JsonRPC = "2.0",
                Result = new RPCTxSearchResult
                {
                    Txs = new List<RPCTxResult>()
                    {
                        new RPCTxResult
                        {
                            Height = "1047",
                            Hash = "098E70A9529AC8F1A57AA0FE65D1D13040B0E803AB8BE7F3B32098164009DED3",
                            TxResult = new TxResult_RPC
                            {
                                Code = 0,
                                Data = "CgYKBHNlbmQ=",
                                Log = "[{'events:[{'type:'bond','attributes:[{'key:'amount','value:'100000000'},{'key:'bound_type','value:'\\u0000'},{'key:'id','value:'46A44C8556375FC41E7B44D1B796995DB2824D7F9C9FD25EA43B2A48493F365F'},{'key:'chain','value:'THOR'},{'key:'from','value:'tthor13gym97tmw3axj3hpewdggy2cr288d3qffr8skg'},{'key:'to','value:'tthor1g98cy3n9mmjrpn0sxmn63lztelera37nrytwp2'},{'key:'coin','value:'100000000 THOR.RUNE'},{'key:'memo','value:'BOND:tthor13gym97tmw3axj3hpewdggy2cr288d3qffr8skg'}]},{'type:'message','attributes:[{'key:'action','value:'deposit'},{'key:'sender','value:'tthor13gym97tmw3axj3hpewdggy2cr288d3qffr8skg'},{'key:'sender','value:'tthor13gym97tmw3axj3hpewdggy2cr288d3qffr8skg'}]},{'type:'new_node','attributes:[{'key:'address','value:'tthor13gym97tmw3axj3hpewdggy2cr288d3qffr8skg'}]},{'type:'transfer','attributes:[{'key:'recipient','value:'tthor1dheycdevq39qlkxs2a6wuuzyn4aqxhve3hhmlw'},{'key:'sender','value:'tthor13gym97tmw3axj3hpewdggy2cr288d3qffr8skg'},{'key:'amount','value:'100000000rune'},{'key:'recipient','value:'tthor17gw75axcnr8747pkanye45pnrwk7p9c3uhzgff'},{'key:'sender','value:'tthor13gym97tmw3axj3hpewdggy2cr288d3qffr8skg'},{'key:'amount','value:'100000000rune'}]}]}]",
                                Info = "",
                                GasWanted = "100000000",
                                GasUsed = "134091",
                                Events = new List<TxEvent>
                                {
                                    new TxEvent
                                    {
                                        Type = "message",
                                        Attributes = new List<TxEventAttribute>
                                        {
                                            new TxEventAttribute
                                            {
                                                Key = "action",
                                                Value = "native_tx"
                                            },
                                            new TxEventAttribute
                                            {
                                                Key = "sender",
                                                Value = "tthor1dspn8ucrqfrnuxrgd5ljuc4elarurt0gkwxgly"
                                            },
                                            new TxEventAttribute
                                            {
                                                Key = "sender",
                                                Value = "tthor1dspn8ucrqfrnuxrgd5ljuc4elarurt0gkwxgly"
                                            }
                                        }
                                    },
                                    new TxEvent
                                    {
                                        Type = "transfer",
                                        Attributes = new List<TxEventAttribute>
                                        {
                                            new TxEventAttribute
                                            {
                                                Key = "recipient",
                                                Value = "tthor1dheycdevq39qlkxs2a6wuuzyn4aqxhve3hhmlw"
                                            },
                                            new TxEventAttribute
                                            {
                                                Key = "sender",
                                                Value = "tthor1dspn8ucrqfrnuxrgd5ljuc4elarurt0gkwxgly"
                                            },
                                            new TxEventAttribute
                                            {
                                                Key = "amount",
                                                Value = "100000000rune"
                                            },
                                            new TxEventAttribute
                                            {
                                                Key = "recipient",
                                                Value = "tthor1g98cy3n9mmjrpn0sxmn63lztelera37nrytwp2"
                                            },
                                            new TxEventAttribute
                                            {
                                                Key = "sender",
                                                Value = "tthor1dspn8ucrqfrnuxrgd5ljuc4elarurt0gkwxgly"
                                            },
                                            new TxEventAttribute
                                            {
                                                Key = "amount",
                                                Value = "200000000000rune"
                                            }
                                        }
                                    }
                                },
                                CodeSpace = ""
                            },
                            Tx = "CoEBCn8KES90eXBlcy5Nc2dEZXBvc2l0EmoKHwoSCgRUSE9SEgRSVU5FGgRSVU5FEgkxMDAwMDAwMDASMUJPTkQ6dHRob3IxM2d5bTk3dG13M2F4ajNocGV3ZGdneTJjcjI4OGQzcWZmcjhza2caFIoJsvl7dHppRuHLmoQRWBqOdsQJElcKTgpGCh8vY29zbW9zLmNyeXB0by5zZWNwMjU2azEuUHViS2V5EiMKIQI7KJDfjLCF1rQl8Dkb+vy9y1HjyC3FM1Qor9zkqywxFRIECgIIfxIFEIDC1y8aQNjQOr84kb74rCRs8TrwVhN89ftC80/6ZC+E9Oh3PVHxS3ngq6vtS3e+jJQXJqf2+1UVSpNZPhxVgxWbIpQRodQ="
                        }
                    },
                    TotalCount = "1"
                }
            });

            var mocks = new Dictionary<Expression<Func<HttpRequestMessage, bool>>, HttpResponseMessage>
            { {
                    x => x.Method == HttpMethod.Get
                          && x.RequestUri.OriginalString.StartsWith($"{this.thorClient.ClientUrl.GetByNetwork(this.thorClient.Network.Value).RPC}" +
                          $"/tx_search"),
                    new HttpResponseMessage
                    {
                        Content = new StringContent(tempres)
                    }
                }
            };

            var hash = "098E70A9529AC8F1A57AA0FE65D1D13040B0E803AB8BE7F3B32098164009DED3";

            tempres = JsonSerializer.Serialize(new TxResponse
            {
                Data = "0A060A0473656E64",
                GasUsed = "35000",
                GasWanted = "200000",
                Height = "0",
                RawLog = "",
                TimeStamp = DateTime.Now.ToString(),
                TxHash = "098E70A9529AC8F1A57AA0FE65D1D13040B0E803AB8BE7F3B32098164009DED3",
                Tx = new StdTx
                {
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
                                            Amount = "100000000",
                                            Denom = "rune"
                                        }
                                    },
                                    FromAddress = AccAddress.FromBech32("tthor13gym97tmw3axj3hpewdggy2cr288d3qffr8skg"),
                                    ToAddress = AccAddress.FromBech32("tthor13gym97tmw3axj3hpewdggy2cr288d3qffr8skg")
                                }
                            }
                    },
                    Fee = new StdTxFee
                    {
                        Amount = new List<Coin>(),
                        Gas = "200000"
                    },
                    Signatures = null,
                    Memo = ""
                }
            });

            mocks.Add(x =>x.Method == HttpMethod.Get
                        && x.RequestUri.OriginalString == $"{this.thorClient.ClientUrl.GetByNetwork(this.thorClient.Network.Value).Node}/txs/{hash}",
                    new HttpResponseMessage
                    {
                        Content = new StringContent(tempres)
                    });



            var httpClient = MockHttpClient(mocks);

            GlobalHttpClient.HttpClient = new HttpClient(httpClient.Object);

            var result = await this.thorClient.GetTransactions(new TxHistoryParamFilter
            {
                Address = "tthor13gym97tmw3axj3hpewdggy2cr288d3qffr8skg"
            });

            httpClient.Protected().Verify("SendAsync", Times.Exactly(2), ItExpr.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Get
                         && x.RequestUri.OriginalString.StartsWith($"{this.thorClient.ClientUrl.GetByNetwork(this.thorClient.Network.Value).RPC}" +
                         $"/tx_search")), ItExpr.IsAny<CancellationToken>());

            Assert.Equal(1, result.Total);
            Assert.Equal(TxType.transfer, result.Txs[0].Type);
            Assert.Equal("098E70A9529AC8F1A57AA0FE65D1D13040B0E803AB8BE7F3B32098164009DED3", result.Txs[0].Hash);
            Assert.IsType<AssetRune>(result.Txs[0].Asset);
            Assert.Equal("tthor13gym97tmw3axj3hpewdggy2cr288d3qffr8skg", result.Txs[0].From[0].From);
            Assert.Equal(100000000, result.Txs[0].From[0].Amount);
            Assert.Equal("tthor13gym97tmw3axj3hpewdggy2cr288d3qffr8skg", result.Txs[0].To[0].To);
            Assert.Equal(100000000, result.Txs[0].To[0].Amount);
        }

        [Fact]
        public async Task Transfer()
        {
            var toAddress = "tthor19kacmmyuf2ysyvq3t9nrl9495l5cvktj5c4eh4";
            var sendAmount = 10000;
            var memo = "transfer";

            var expected = new BroadcastTxCommitResult
            {
                CheckTxResult = new CheckTxResult(),
                DeliverTxResult = new DeliverTxResult(),
                TxHash = "EA2FAC9E82290DCB9B1374B4C95D7C4DD8B9614A96FACD38031865EB1DBAE24D",
                Height = "0",
                Logs = new List<object>()
            };
            var expectedTxsPostResult = JsonSerializer.Serialize(expected);

            var tempres = JsonSerializer.Serialize(new CommonResponse<BaseAccount>
            {
                Height = "0",
                Result = new BaseAccount
                {
                    AccountNumber = "0",
                    Sequence = "0",
                    Coins = new List<Coin>
                    {
                        new Coin
                        {
                            Amount = "210000000",
                            Denom = "rune"
                        }
                    }
                }
            });

            var mocks = new Dictionary<Expression<Func<HttpRequestMessage, bool>>, HttpResponseMessage>
            {
                {
                    x =>
                        x.Method == HttpMethod.Get
                        && x.RequestUri.OriginalString == $"{this.thorClient.ClientUrl.GetByNetwork(this.thorClient.Network.Value).Node}" +
                        $"/auth/accounts/{testnet_address_path0}",
                    new HttpResponseMessage
                    {
                        Content = new StringContent(tempres)
                    }
                }
            };

            tempres = JsonSerializer.Serialize(new CommonResponse<List<Coin>>
            {
                Height = "0",
                Result = new List<Coin>()
                {
                    new Coin
                    {
                        Amount = "210000000",
                        Denom = "rune"
                    }
                }
            });

            mocks.Add(
                    x => x.Method == HttpMethod.Get
                          && x.RequestUri.OriginalString == $"{this.thorClient.ClientUrl.GetByNetwork(this.thorClient.Network.Value).Node}" +
                          $"/bank/balances/{testnet_address_path0}",
                    new HttpResponseMessage
                    {
                        Content = new StringContent(tempres)
                    }
              );


            mocks.Add(x =>
                       x.Method == HttpMethod.Post
                       && x.RequestUri.OriginalString == $"{this.thorClient.ClientUrl.GetByNetwork(this.thorClient.Network.Value).Node}/txs"
                       , new HttpResponseMessage
                       {
                           Content = new StringContent(expectedTxsPostResult)
                       });



            var httpClient = MockHttpClient(mocks);

            GlobalHttpClient.HttpClient = new HttpClient(httpClient.Object);

            var res = await this.thorClient.Transfer(new TxParams
            {
                Asset = new AssetRune(),
                Recipient = toAddress,
                Amount = sendAmount,
                Memo = memo
                
            });
            Assert.Equal("EA2FAC9E82290DCB9B1374B4C95D7C4DD8B9614A96FACD38031865EB1DBAE24D", res);
            Assert.Single(txsPost.Tx.Msg);
            Assert.Equal("transfer", txsPost.Tx.Memo);
            Assert.Equal(testnet_address_path0, ((AminoWrapper<MsgSend>)txsPost.Tx.Msg[0]).Value.FromAddress.ToBech32());
            Assert.Equal("tthor19kacmmyuf2ysyvq3t9nrl9495l5cvktj5c4eh4", ((AminoWrapper<MsgSend>)txsPost.Tx.Msg[0]).Value.FromAddress.ToBech32());
            Assert.Equal("10000", ((AminoWrapper<MsgSend>)txsPost.Tx.Msg[0]).Value.Amount[0].Amount);
            Assert.Equal("rune", ((AminoWrapper<MsgSend>)txsPost.Tx.Msg[0]).Value.Amount[0].Denom);
            Assert.Equal("thorchain/MsgSend", ((AminoWrapper<MsgSend>)txsPost.Tx.Msg[0]).Type);
        }


        [Fact]
        public async Task Deposit()
        {
            var sendAmount = 10000;
            var memo = "swap:BNB.BNB:tbnb1ftzhmpzr4t8ta3etu4x7nwujf9jqckp3th2lh0";

            var expected = new BroadcastTxCommitResult
            {
                CheckTxResult = new CheckTxResult(),
                DeliverTxResult = new DeliverTxResult(),
                TxHash = "EA2FAC9E82290DCB9B1374B4C95D7C4DD8B9614A96FACD38031865EB1DBAE24D",
                Height = "0",
                Logs = new List<object>(),
                Data = "data",
                GasUsed = "gasused",
                GasWanted = "gaswanted",
                RawLog = "rawlog"
            };
            var expectedTxsPostResult = JsonSerializer.Serialize(expected);

            var tempres = JsonSerializer.Serialize(new CommonResponse<BaseAccount>
            {
                Height = "0",
                Result = new BaseAccount
                {
                    AccountNumber = "0",
                    Sequence = "0",
                    Coins = new List<Coin>
                    {
                        new Coin
                        {
                            Amount = "210000000",
                            Denom = "rune"
                        }
                    }
                }
            });

            var mocks = new Dictionary<Expression<Func<HttpRequestMessage, bool>>, HttpResponseMessage>
            {
                {
                    x =>
                        x.Method == HttpMethod.Get
                        && x.RequestUri.OriginalString == $"{this.thorClient.ClientUrl.GetByNetwork(this.thorClient.Network.Value).Node}" +
                        $"/auth/accounts/{testnet_address_path0}",
                    new HttpResponseMessage
                    {
                        Content = new StringContent(tempres)
                    }
                }
            };

            tempres = JsonSerializer.Serialize(new CommonResponse<List<Coin>>
            {
                Height = "0",
                Result = new List<Coin>()
                {
                    new Coin
                    {
                        Amount = "210000000",
                        Denom = "rune"
                    }
                }
            });

            mocks.Add(
                    x => x.Method == HttpMethod.Get
                          && x.RequestUri.OriginalString == $"{this.thorClient.ClientUrl.GetByNetwork(this.thorClient.Network.Value).Node}" +
                          $"/bank/balances/{testnet_address_path0}",
                    new HttpResponseMessage
                    {
                        Content = new StringContent(tempres)
                    }
              );


            mocks.Add(x =>
                       x.Method == HttpMethod.Post
                       && x.RequestUri.OriginalString == $"{this.thorClient.ClientUrl.GetByNetwork(this.thorClient.Network.Value).Node}/txs"
                       , new HttpResponseMessage
                       {
                           Content = new StringContent(expectedTxsPostResult)
                       });

            var expectedDepositResult = JsonSerializer.Serialize(new ThorchainDepositResponse
            {
                Type = "cosmos-sdk/StdTx",
                Value = new StdTx
                {
                    Msg = new List<Msg>
                    {
                        new AminoWrapper<MsgDeposit>
                        {
                            Type = "thorchain/MsgDeposit",
                            Value = new MsgDeposit
                            {
                                Coins = new List<MsgCoinDeposit>
                                {
                                    new MsgCoinDeposit
                                    {
                                        Amount = "10000",
                                        Asset = "THOR.RUNE"
                                    }
                                },
                                Memo = "swap:BNB.BNB:tbnb1ftzhmpzr4t8ta3etu4x7nwujf9jqckp3th2lh0",
                                Signer = AccAddress.FromBech32("tthor19kacmmyuf2ysyvq3t9nrl9495l5cvktj5c4eh4")
                            }
                        }
                    },
                    Fee = new StdTxFee
                    {
                        Amount = new List<Coin>(),
                        Gas = "100000000"
                    },
                    Signatures = new List<StdSignature>(),
                    Memo = "",
                    TimeoutHeight = "0"
                }
            });

            mocks.Add(x =>
                       x.Method == HttpMethod.Post
                       && x.RequestUri.OriginalString == $"{this.thorClient.ClientUrl.GetByNetwork(this.thorClient.Network.Value).Node}/thorchain/deposit"
                       , new HttpResponseMessage
                       {
                           Content = new StringContent(expectedDepositResult)
                       });



            var httpClient = MockHttpClient(mocks);

            GlobalHttpClient.HttpClient = new HttpClient(httpClient.Object);

            var res = await this.thorClient.Deposit(new DepositParam
            {
                Asset = new AssetRune(),
                Amount = sendAmount,
                Memo = memo
            });
            Assert.Equal("EA2FAC9E82290DCB9B1374B4C95D7C4DD8B9614A96FACD38031865EB1DBAE24D", res);
        }

        [Fact]
        public async Task Get_Transaction_Data()
        {
            var hash = "19BFC1E8EBB10AA1EC6B82E380C6F5FD349D367737EA8D55ADB4A24F0F7D1066";
            var tempres = JsonSerializer.Serialize(new TxResponse
            {
                Data = "0A060A0473656E64",
                GasUsed = "35000",
                GasWanted = "200000",
                Height = "0",
                RawLog = "",
                TimeStamp = DateTime.Now.ToString(),
                TxHash = "19BFC1E8EBB10AA1EC6B82E380C6F5FD349D367737EA8D55ADB4A24F0F7D1066",
                Tx = new StdTx
                {
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
                                            Amount = "100000000",
                                            Denom = "rune"
                                        }
                                    },
                                    FromAddress = AccAddress.FromBech32("tthor13gym97tmw3axj3hpewdggy2cr288d3qffr8skg"),
                                    ToAddress = AccAddress.FromBech32("tthor13gym97tmw3axj3hpewdggy2cr288d3qffr8skg")
                                }
                            }
                    },
                    Fee = new StdTxFee
                    {
                        Amount = new List<Coin>(),
                        Gas = "200000"
                    },
                    Signatures = null,
                    Memo = ""
                }
            });
            var mocks = new Dictionary<Expression<Func<HttpRequestMessage, bool>>, HttpResponseMessage>
            {
                {
                    x =>
                        x.Method == HttpMethod.Get
                        && x.RequestUri.OriginalString == $"{this.thorClient.ClientUrl.GetByNetwork(this.thorClient.Network.Value).Node}/txs/{hash}",
                    new HttpResponseMessage
                    {
                        Content = new StringContent(tempres)
                    }
                }
            };

            var httpMock = MockHttpClient(mocks);
            GlobalHttpClient.HttpClient = new HttpClient(httpMock.Object);

            var tx = await this.thorClient.GetTranasctionData("19BFC1E8EBB10AA1EC6B82E380C6F5FD349D367737EA8D55ADB4A24F0F7D1066");

            Assert.Equal(TxType.transfer, tx.Type);
            Assert.Equal("19BFC1E8EBB10AA1EC6B82E380C6F5FD349D367737EA8D55ADB4A24F0F7D1066", tx.Hash);
            Assert.IsType<AssetRune>(tx.Asset);
            Assert.Equal("tthor13gym97tmw3axj3hpewdggy2cr288d3qffr8skg", tx.From[0].From);
            Assert.Equal(100000000, tx.From[0].Amount);
            Assert.Equal("tthor13gym97tmw3axj3hpewdggy2cr288d3qffr8skg", tx.To[0].To);
            Assert.Equal(100000000, tx.To[0].Amount);
        }

    }
}
