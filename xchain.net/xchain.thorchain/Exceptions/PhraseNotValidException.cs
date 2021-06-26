using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchain.net.xchain.thorchain.Exceptions
{
    public class PhraseNotValidException : Exception
    {
        public string Phrase { get; set; }

        public PhraseNotValidException(string phrase) : base()
        {
            this.Phrase = phrase;
        }

        public PhraseNotValidException(string phrase , string message) : base(message)
        {
            this.Phrase = phrase;
        }


    }
}
