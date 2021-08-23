namespace XchainDotnet.Cosmos.Models.Crypto
{
    public interface IPublicKey
    {
        /// <summary>
        /// Get Address from current public key
        /// </summary>
        /// <returns>address byte array</returns>
        byte[] GetAddress();
        /// <summary>
        /// Get public key as byte array
        /// </summary>
        /// <returns>Public key byte array</returns>
        byte[] ToBuffer();
        /// <summary>
        /// Base64 representation of the current public key
        /// </summary>
        /// <returns>Base64 string</returns>
        string ToBase64();
        /// <summary>
        /// Verify input signature of the message using currnet public key
        /// </summary>
        /// <param name="signature">message signature</param>
        /// <param name="message">message</param>
        /// <returns>true or false</returns>
        bool Verify(byte[] signature, byte[] message);
    }
}
