using dotnetstandard_bip39;
using NBitcoin;

namespace XchainDotnet.Crypto
{
    public class XcahinCrypto
    {
        public static bool ValidatePhrase(string phrase)
        {
            BIP39 bip39 = new();

            return bip39.ValidateMnemonic(phrase, BIP39Wordlist.English);
        }

        public static byte[] GetSeed(string phrase)
        {
            Mnemonic mnemonic = new(phrase);
            var res = mnemonic.DeriveSeed();
            return res;
        }
    }
}
