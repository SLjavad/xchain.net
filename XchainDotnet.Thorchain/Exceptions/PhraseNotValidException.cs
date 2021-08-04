using System;

namespace XchainDotnet.Thorchain.Exceptions
{
    public class PhraseNotValidException : Exception
    {
        public string Phrase { get; set; }

        public PhraseNotValidException(string phrase) : base()
        {
            Phrase = phrase;
        }

        public PhraseNotValidException(string phrase, string message) : base(message)
        {
            Phrase = phrase;
        }


    }
}
