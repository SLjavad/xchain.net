using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xchain.net.xchain.client;
using Xchain.net.xchain.client.Models;
using Xchain.net.xchain.cosmos.Models.Address;
using Xchain.net.xchain.cosmos.Models.Message;
using Xchain.net.xchain.cosmos.Models.Message.Base;
using Xchain.net.xchain.cosmos.Models.Tx;
using Xchain.net.xchain.thorchain.Constants;
using Xchain.net.xchain.thorchain.Models;

namespace xchain.net.xchain.thorchain
{
    public class ThorchainUtils
    {
        public static ClientUrl GetDefaultClientUrl()
        {
            return new ClientUrl()
            {
                Mainnet = new NodeUrl("https://thornode.thorchain.info", "https://rpc.thorchain.info"),
                Testnet = new NodeUrl("https://testnet.thornode.thorchain.info", "https://testnet.rpc.thorchain.info")
            };
        }

        public static ExplorerUrl GetDefaultExplorerUrl()
        {
            return new ExplorerUrl
            {
                Testnet = "https://testnet.thorchain.net/#",
                Mainnet = "https://thorchain.net/#"
            };
        }

        public static string GetPrefix(Network network) => network switch
        {
            Network.mainnet => "thor",
            Network.testnet => "tthor",
            _ => throw new Exception("Invalid Network"),
        };

        public static Asset GetAsset(string denom)
        {
            if (denom == GetDenom(new AssetRune()))
            {
                return new AssetRune();
            }
            return Utils.AssetFromString($"THOR.{denom.ToUpperInvariant()}");
        }

        public static string GetDenom(Asset asset)
        {
            if (Utils.AssetToString(asset) == Utils.AssetToString(new AssetRune()))
            {
                return "rune";
            }
            else
            {
                return asset.Symbol;
            }
        }

        private static byte[] StringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static string GetTxType(string txData, string encoding)
        {
            switch (encoding)
            {
                case "base64":
                    {
                        var res = Encoding.UTF8.GetString(Convert.FromBase64String(txData));
                        res = res[4..];
                        return res;
                    }
                case "hex":
                    {
                        var res = Encoding.UTF8.GetString(StringToByteArray(txData));
                        res = res[4..];
                        return res;
                    }
                default:
                    return null;
            }
        }

        public static bool IsMsgSend(IMsg msg)
        {
            if (msg == null)
            {
                return false;
            }
            if (msg is MsgSend msgSend)
            {
                return msgSend.Amount != null && msgSend.FromAddress != null && msgSend.ToAddress != null;
            }
            return false;
        }

        public static bool IsMsgMultiSend(IMsg msg)
        {
            if (msg == null)
            {
                return false;
            }
            if (msg is MsgMultiSend msgMultiSend)
            {
                return msgMultiSend.Inputs != null && msgMultiSend.Outputs != null;
            }
            return false;
        }

        public static List<Tx> GetTxFromHistory(List<TxResponse> txs, Network network)
        {
            var prefix = GetPrefix(network);
            AccAddress.SetBech32Prefix(prefix,
                                        prefix + "pub",
                                        prefix + "valoper",
                                        prefix + "valoperpub",
                                        prefix + "valcons",
                                        prefix + "valconspub"
                                        );

            List<Tx> txes = new();
            var result = txs.Aggregate(txes, (acc, tx) =>
               {
                   List<IMsg> msgs = new();

                   if (tx.Tx is not RawTxResponse)
                   {
                       msgs = new((tx.Tx as StdTx).Msg);
                   }
                   else
                   {
                       msgs = new((tx.Tx as RawTxResponse).Body.Messages);
                   }

                   List<TxFrom> froms = new();
                   List<TxTo> tos = new();

                   msgs.ForEach(msg =>
                   {
                       if (IsMsgSend(msg))
                       {
                           var msgSend = msg as MsgSend;
                           //var amount = msgSend.Amount.Select(coin => coin.Amount).Aggregate((decimal)0, (acc, cur) => acc + cur);
                           var amount = msgSend.Amount.Sum(coin => coin.Amount);

                           var from_index = -1;

                           if ((from_index = froms.FindIndex(x => x.From == msgSend.FromAddress.ToBech32())) == -1)
                           {
                               froms.Add(new TxFrom
                               {
                                   From = msgSend.FromAddress.ToBech32(),
                                   Amount = amount
                               });
                           }
                           else
                           {
                               froms[from_index].Amount += amount;
                           }

                           var to_index = 0;

                           if ((to_index = tos.FindIndex(x => x.To == msgSend.ToAddress.ToBech32())) == -1)
                           {
                               tos.Add(new TxTo
                               {
                                   To = msgSend.ToAddress.ToBech32(),
                                   Amount = amount
                               });
                           }
                           else
                           {
                               tos[to_index].Amount += amount;
                           }
                       }
                       else if (IsMsgMultiSend(msg))
                       {
                           var msgMultiSend = msg as MsgMultiSend;

                           msgMultiSend.Inputs.ForEach(inp =>
                           {
                               var amount = inp.Coins.Sum(coin => coin.Amount);

                               var from_index = -1;

                               if ((from_index = froms.FindIndex(x => x.From == inp.Address)) == -1)
                               {
                                   froms.Add(new TxFrom
                                   {
                                       From = inp.Address,
                                       Amount = amount
                                   });
                               }
                               else
                               {
                                   froms[from_index].Amount += amount;
                               }
                           });

                           msgMultiSend.Outputs.ForEach(output =>
                           {
                               var amount = output.Coins.Sum(coin => coin.Amount);

                               var to_index = -1;

                               if ((to_index = tos.FindIndex(x => x.To == output.Address)) == -1)
                               {
                                   tos.Add(new TxTo
                                   {
                                       To = output.Address,
                                       Amount = amount
                                   });
                               }
                               else
                               {
                                   tos[to_index].Amount += amount;
                               }
                           });
                       }
                   });
                   acc.Add(new Tx
                   {
                       Asset = new AssetRune(),
                       From = froms,
                       To = tos,
                       Date = DateTime.Parse(tx.TimeStamp),
                       Hash = tx.TxHash,
                       Type = froms.Count > 0 || tos.Count > 0 ? TxType.transfer : TxType.unknown
                   });
                   return acc;
               });
            return result;
        }

        public static Fees GetDefaultFees()
        {
            var fee = ThorchainConstantValues.DEFAULT_GAS_VALUE;

            return new Fees
            {
                Type = FeeType.@base,
                Average = fee,
                Fast = fee,
                Fastest = fee
            };
        }
    }
}
