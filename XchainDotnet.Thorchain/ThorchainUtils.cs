using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XchainDotnet.Client;
using XchainDotnet.Client.Models;
using XchainDotnet.Cosmos.Models;
using XchainDotnet.Cosmos.Models.Address;
using XchainDotnet.Cosmos.Models.Message;
using XchainDotnet.Cosmos.Models.Message.Base;
using XchainDotnet.Cosmos.Models.Tx;
using XchainDotnet.Thorchain.Constants;
using XchainDotnet.Thorchain.Models;

namespace XchainDotnet.Thorchain
{
    /// <summary>
    /// Utils for thorchain
    /// </summary>
    public class ThorchainUtils
    {
        public const string DEFAULT_EXPLORER_URL = "https://viewblock.io/thorchain";

        /// <summary>
        /// Get Default Client URL
        /// </summary>
        /// <returns>Default <see cref="ClientUrl"/> Object</returns>
        public static ClientUrl GetDefaultClientUrl()
        {
            return new ClientUrl()
            {
                Mainnet = new NodeUrl("https://thornode.thorchain.info", "https://rpc.thorchain.info"),
                Testnet = new NodeUrl("https://testnet.thornode.thorchain.info", "https://testnet.rpc.thorchain.info")
            };
        }

        /// <summary>
        /// Get Default Explorer URL
        /// </summary>
        /// <returns>Default <see cref="ExplorerUrl"/> Object</returns>
        public static ExplorerUrls GetDefaultExplorerUrl()
        {
            return new ExplorerUrls
            {
                Address = new ExplorerUrl
                {
                    Mainnet = $"{DEFAULT_EXPLORER_URL}/address",
                    Testnet = $"{DEFAULT_EXPLORER_URL}/address"
                },
                Root = new ExplorerUrl
                {
                    Testnet = $"{DEFAULT_EXPLORER_URL}?network=testnet",
                    Mainnet = DEFAULT_EXPLORER_URL
                },
                Tx = new ExplorerUrl
                {
                    Mainnet = $"{DEFAULT_EXPLORER_URL}/tx",
                    Testnet = $"{DEFAULT_EXPLORER_URL}/tx"
                }
            };
        }

        /// <summary>
        /// Get Prefix based on network
        /// </summary>
        /// <param name="network">network type</param>
        /// <returns>prefix string</returns>
        public static string GetPrefix(Network network) => network switch
        {
            Network.mainnet => "thor",
            Network.testnet => "tthor",
            _ => throw new Exception("Invalid Network"),
        };

        /// <summary>
        /// Get Asset from denomination
        /// </summary>
        /// <param name="denom">input denom</param>
        /// <returns>The asset of the given denomination</returns>
        public static Asset GetAsset(string denom)
        {
            if (denom == GetDenom(new AssetRune()))
            {
                return new AssetRune();
            }
            return Utils.AssetFromString($"THOR.{denom.ToUpperInvariant()}");
        }

        /// <summary>
        /// Get denomination from Asset
        /// </summary>
        /// <param name="asset">input asset</param>
        /// <returns>The denomination of the given asset</returns>
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

        /// <summary>
        /// Get denomination with chainname from Asset
        /// </summary>
        /// <param name="asset">input asset</param>
        /// <returns>The denomination with chainname of the given asset</returns>
        public static string GetDenomWithChain(Asset asset)
        {
            return $"{Chain.THOR}.{asset.Symbol.ToUpperInvariant()}";
        }

        private static byte[] StringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        /// <summary>
        /// Get transaction type.
        /// </summary>
        /// <param name="txData">the transaction input data</param>
        /// <param name="encoding">`base64` or `hex`</param>
        /// <returns>the transaction type</returns>
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

        /// <summary>
        /// Type guard for MsgSend
        /// </summary>
        /// <param name="msg">input msg</param>
        /// <returns>true or false</returns>
        public static bool IsMsgSend(Msg msg)
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

        /// <summary>
        /// Type guard for MsgMultiSend
        /// </summary>
        /// <param name="msg">input msg</param>
        /// <returns>true or false</returns>
        public static bool IsMsgMultiSend(Msg msg)
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

        /// <summary>
        /// Parse transaction type
        /// </summary>
        /// <param name="txs">The transaction response from the node</param>
        /// <param name="network">network</param>
        /// <returns>list of parsed transaction</returns>
        public static List<Tx> GetTxsFromHistory(List<TxResponse> txs, Network network)
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
                   List<Msg> msgs = new();

                   switch (tx.Tx)
                   {
                       case RawTxResponse rawTxResponse:
                           msgs = new((tx.Tx as RawTxResponse).Body.Messages);
                           break;
                       case StdTx stdTx:
                           msgs = new((tx.Tx as StdTx).Msg);
                           break;
                       case AminoWrapper<StdTx> aminoTx:
                           msgs = new((tx.Tx as AminoWrapper<StdTx>).Value.Msg);
                           break;
                       default:
                           break;
                   }
                   msgs = msgs.Select(x =>
                   {
                       if (x.GetType().GetGenericTypeDefinition() == typeof(AminoWrapper<>))
                       {
                           return (Msg)((dynamic)x).Value;
                       }
                       return x;
                   }).ToList();

                   List<TxFrom> froms = new();
                   List<TxTo> tos = new();

                   msgs.ForEach(msg =>
                   {
                       if (IsMsgSend(msg))
                       {
                           var msgSend = msg as MsgSend;
                           //var amount = msgSend.Amount.Select(coin => coin.Amount).Aggregate((decimal)0, (acc, cur) => acc + cur);
                           var amount = msgSend.Amount.Sum(coin => decimal.Parse(coin.Amount));

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
                               var amount = inp.Coins.Sum(coin => decimal.Parse(coin.Amount));

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
                               var amount = output.Coins.Sum(coin => decimal.Parse(coin.Amount));

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

        /// <summary>
        /// Get the default fee.
        /// </summary>
        /// <returns>The default fee</returns>
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

        /// <summary>
        /// Response guard for transaction broadcast
        /// </summary>
        /// <param name="result">The response from the node</param>
        /// <returns>true or false</returns>
        public static bool IsBroadcastSuccess(BroadcastTxCommitResult result) => result.Logs != null;
    }
}
