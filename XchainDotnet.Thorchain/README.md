# `XchainDotnet.Thorchain`

Thorchain Module for XChainDotnet Clients

## Installation

```
dotnet add package XchainDotnet.Thorchain
```
The Package is based on latest .net (.net5) so it supports .netcore 3.1 and .net5

## Examples

```C#
// using xchainDotnet.thorchain
using XchainDotnet.Thorchain;


// Create a `Client`
var client = new ThorchainClient(mnemonic, null, null, Network.testnet);

// get address
var address = client.GetAddress()
Console.WriteLine('address:'+ address) // address: tthor13gym97tmw3axj3hpewdggy2cr288d3qffr8skg

// get balances
var balances = await client.GetBalance(address)
Console.WriteLine('balances: '+ balances[0].Amount) // balance: 6968080395099

// get transactions
var result = await client.GetTransactions(new TxHistoryParamFilter
            {
                Address = "tthor13gym97tmw3axj3hpewdggy2cr288d3qffr8skg",
                Limit = 1
            });
Console.WriteLine('txs total:', txs.total) // txs total: 100

// get transaction details
var tx = await client.GetTranasctionData("19BFC1E8EBB10AA1EC6B82E380C6F5FD349D367737EA8D55ADB4A24F0F7D1066");
Console.WriteLine(tx.Type); // transfer
Console.WriteLine(tx.Hash); //"19BFC1E8EBB10AA1EC6B82E380C6F5FD349D367737EA8D55ADB4A24F0F7D1066"
Console.WriteLine(tx.Asset); //{ chain: 'THOR', symbol: 'RUNE', ticker: 'RUNE' }
Console.WriteLine(tx.From[0].From); // tthor13gym97tmw3axj3hpewdggy2cr288d3qffr8skg
Console.WriteLine(tx.From[0].Amount); // 100000000
Console.WriteLine(tx.To[0].To); //tthor13gym97tmw3axj3hpewdggy2cr288d3qffr8skg
Console.WriteLine(tx.To[0].Amount); // 100000000
```

For more examples check out tests in `./XchainDotnet.Thorchain.Tests/XchainThorcahinTest.cs`
