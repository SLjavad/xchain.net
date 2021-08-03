namespace XchainDotnet.Cosmos.Models.Crypto.RIPEMD160
{
    public abstract class RIPEMD160Crypto : System.Security.Cryptography.HashAlgorithm
    {
        public RIPEMD160Crypto()
        {
        }

        public new static RIPEMD160Crypto Create()
        {
            return new RIPEMD160Managed();
        }

        public new static RIPEMD160Crypto Create(string hashname)
        {
            return new RIPEMD160Managed();
        }
    }
}
