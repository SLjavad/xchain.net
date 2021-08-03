namespace XchainDotnet.Cosmos.Models.Crypto
{
    public interface IPublicKey
    {
        byte[] GetAddress();
        byte[] ToBuffer();
        string ToBase64();
        bool Verify(byte[] signature, byte[] message);
    }
}
