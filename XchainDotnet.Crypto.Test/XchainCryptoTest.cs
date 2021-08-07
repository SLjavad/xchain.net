using System;
using Xunit;

namespace XchainDotnet.Crypto.Test
{
    public class XchainCryptoTest
    {
        [Fact]
        public void Generate_12_Wrods_Phrase()
        {
            var phrase = XchainCrypto.GeneratePhrase();
            var words = phrase.Split(" ");
            Assert.Equal(12, words.Length);
        }
        [Fact]
        public void Generate_24_Wrods_Phrase()
        {
            var phrase = XchainCrypto.GeneratePhrase(24);
            var words = phrase.Split(" ");
            Assert.Equal(24, words.Length);
        }

        [Fact]
        public void Validate_12_Wrods_Phrase()
        {
            var phrase = XchainCrypto.GeneratePhrase();
            var valid = XchainCrypto.ValidatePhrase(phrase);
            Assert.True(valid);
        }
        [Fact]
        public void Validate_24_Wrods_Phrase()
        {
            var phrase = XchainCrypto.GeneratePhrase(24);
            var valid = XchainCrypto.ValidatePhrase(phrase);
            Assert.True(valid);
        }

        [Fact]
        public void Invalidate_Wrods_Phrase()
        {
            var phrase = "flush viable fury sword mention dignity ethics secret nasty gallery teach wrong";
            var valid = XchainCrypto.ValidatePhrase(phrase);
            Assert.False(valid);
        }
    }
}
