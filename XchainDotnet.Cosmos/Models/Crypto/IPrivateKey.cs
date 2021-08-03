namespace XchainDotnet.Cosmos.Models.Crypto
{
    public interface IPrivateKey
    {
        IPublicKey GetPublicKey();
        byte[] ToBuffer();
        string ToBase64();
        byte[] Sign(byte[] message);
    }
}
