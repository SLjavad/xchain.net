namespace XchainDotnet.Cosmos.Models.Crypto
{
    public interface IPrivateKey
    {
        /// <summary>
        /// Get public key from private key
        /// </summary>
        /// <returns>public key object</returns>
        IPublicKey GetPublicKey();
        /// <summary>
        /// Get byte array of the current private key
        /// </summary>
        /// <returns>private key  byte array</returns>
        byte[] ToBuffer();
        /// <summary>
        /// Get Base64 representation of the current private key
        /// </summary>
        /// <returns>base64 string</returns>
        string ToBase64();
        /// <summary>
        /// Sign input message using current private key
        /// </summary>
        /// <param name="message">input message as byte array</param>
        /// <returns>signed message</returns>
        byte[] Sign(byte[] message);
    }
}
