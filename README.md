# Xchain.net
Xchain.net is a library with a common interface for multiple blockchains, built for simple and fast integration for wallets and more.


## Xchain.net uses following libraries, frameworks and more:

- [NBitcoin](https://github.com/MetacoSA/NBitcoin)
- [Cryptography.ECDSA.Secp256K1](https://github.com/Chainers/Cryptography.ECDSA)
- [Secp256k1.Net](https://github.com/MeadowSuite/Secp256k1.Net)
- [dotnetstandard-bip39](https://github.com/elucidsoft/dotnetstandard-bip39)

#### ATTENTION
**All Xchain.net modules Are under `XchainDotnet` Namespace , so if you faced with multiple objects with different namespaces , choose `XchainDotnet` one**

for example: `Coin` class can be found in `XchainDotnet` and `NBitcoin` namespaces , so you should use `XchainDotnet` one :)

#### NOTE
**This library currently supports `Thorchain` and is under develop to support other chains.**

**current modules are designed to implement `Thorchain`.**