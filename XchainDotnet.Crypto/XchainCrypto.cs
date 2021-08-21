using dotnetstandard_bip39;
using NBitcoin;

namespace XchainDotnet.Crypto
{
    public class XchainCrypto
    {
        /// <summary>
        /// Validate input phrase
        /// </summary>
        /// <param name="phrase">input phrase</param>
        /// <returns>true or false</returns>
        public static bool ValidatePhrase(string phrase)
        {
            BIP39 bip39 = new();

            return bip39.ValidateMnemonic(phrase, BIP39Wordlist.English);
        }
        /// <summary>
        /// generate new phrase
        /// </summary>
        /// <param name="size">The new phrase size</param>
        /// <returns>The generated phrase based on the size</returns>
        public static string GeneratePhrase(int size = 12)
        {
            BIP39 bip39 = new();
            var phrase = bip39.GenerateMnemonic(size == 12 ? 128 : 256, BIP39Wordlist.English);
            return phrase;
        }

        /// <summary>
        /// Get the seed from the given phrase
        /// </summary>
        /// <param name="phrase">phrase</param>
        /// <returns>The seed from the given phrase</returns>
        public static byte[] GetSeed(string phrase)
        {
            Mnemonic mnemonic = new(phrase);
            var res = mnemonic.DeriveSeed();
            return res;
        }
    }
}
