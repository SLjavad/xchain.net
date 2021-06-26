using dotnetstandard_bip39;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchain.net.xchain.crypto
{
    public class XcahinCrypto
    {
        public static bool ValidatePhrase(string phrase)
        {
            BIP39 bip39 = new BIP39();

            return bip39.ValidateMnemonic(phrase, BIP39Wordlist.English);
        }
    }
}
