using dotnetstandard_bip39;
using NBitcoin;

namespace XchainDotnet.Crypto
{
    public class XchainCrypto
    {
        public static bool ValidatePhrase(string phrase)
        {
            BIP39 bip39 = new();

            return bip39.ValidateMnemonic(phrase, BIP39Wordlist.English);
        }

        public static string GeneratePhrase(int size = 12)
        {
            BIP39 bip39 = new();
            var phrase = bip39.GenerateMnemonic(size == 12 ? 128 : 256, BIP39Wordlist.English);
            return phrase;
        }

        public static byte[] GetSeed(string phrase)
        {
            Mnemonic mnemonic = new(phrase);
            var res = mnemonic.DeriveSeed();
            return res;
        }
    }
}
