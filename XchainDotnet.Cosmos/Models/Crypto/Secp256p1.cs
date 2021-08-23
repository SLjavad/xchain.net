using Cryptography.ECDSA;
using Secp256k1Net;
using System;
using System.Linq;
using System.Security.Cryptography;
using XchainDotnet.Cosmos.Models.Crypto.RIPEMD160;

namespace XchainDotnet.Cosmos.Models.Crypto
{
    /// <summary>
    /// Secp256k1 implementation of the private key
    /// </summary>
    public class PrivateKeySecp256k1 : IPrivateKey
    {
        private readonly byte[] privKey;
        private PublicKeySecp256k1 pubKey;

        /// <summary>
        /// Secp256k1 implementation of the private key
        /// </summary>
        /// <param name="privKey">input private key as byte array</param>
        public PrivateKeySecp256k1(byte[] privKey)
        {
            pubKey = new PublicKeySecp256k1(Secp256K1Manager.GetPublicKey(privKey, true));
            this.privKey = privKey;
        }

        /// <summary>
        /// Get public key of the current private key
        /// </summary>
        /// <returns>public key object</returns>
        public IPublicKey GetPublicKey()
        {
            return pubKey;
        }

        /// <summary>
        /// Sign input message using current private key
        /// </summary>
        /// <param name="message">input message</param>
        /// <returns>signed message</returns>
        public byte[] Sign(byte[] message)
        {
            var hash = SHA256.Create().ComputeHash(message);
            var signature = Secp256K1Manager.SignCompressedCompact(hash, privKey); //TODO: change lib // first field not needed
            signature = signature.Skip(1).ToArray();

            return signature;
        }

        /// <summary>
        /// Base64 representation of the current private key
        /// </summary>
        /// <returns>Base64 string</returns>
        public string ToBase64()
        {
            return Convert.ToBase64String(privKey);
        }

        /// <summary>
        /// byte array of the current private key
        /// </summary>
        /// <returns>byte array</returns>
        public byte[] ToBuffer()
        {
            return privKey;
        }

        /// <summary>
        /// Base64 encode
        /// </summary>
        /// <returns></returns>
        public string ToJsonInCodec()
        {
            return ToBase64();
        }

        /// <summary>
        /// Get private key from base64 value
        /// </summary>
        /// <param name="value">base64 input value</param>
        /// <returns>Private key object</returns>
        public static IPrivateKey FromBase64(string value)
        {
            var buffer = Convert.FromBase64String(value);
            return new PrivateKeySecp256k1(buffer);
        }

        public static IPrivateKey FromJson(string value)
        {
            return FromBase64(value);
        }
    }

    /// <summary>
    /// Secp256k1 implementation of the public key
    /// </summary>
    public class PublicKeySecp256k1 : IPublicKey
    {
        private readonly byte[] pubKey;

        /// <summary>
        /// Secp256k1 implementation of the public key
        /// </summary>
        /// <param name="pubKey">input public key as byte array</param>
        public PublicKeySecp256k1(byte[] pubKey)
        {
            this.pubKey = pubKey;
        }


        public byte[] GetAddress()
        {
            return Hash160(pubKey);
        }


        public string ToBase64()
        {
            return Convert.ToBase64String(pubKey);
        }


        public byte[] ToBuffer()
        {
            return pubKey;
        }

        /// <summary>
        /// Base 64 string
        /// </summary>
        /// <returns></returns>
        public string ToJsonInCodec()
        {
            return ToBase64();
        }

        /// <summary>
        /// Get public key from base64 value
        /// </summary>
        /// <param name="value">input base 64 value</param>
        /// <returns>Public key object</returns>
        public static IPublicKey FromBase64(string value)
        {
            return new PublicKeySecp256k1(Convert.FromBase64String(value));
        }

        public static IPublicKey FromJSON(string value)
        {
            return FromBase64(value);
        }

        public bool Verify(byte[] signature, byte[] message)
        {
            byte[] hash;
            using (SHA256 sHA256 = SHA256.Create())
            {
                hash = sHA256.ComputeHash(message);
            }

            bool res;
            using (Secp256k1 secp256K1 = new Secp256k1())
            {
                res = secp256K1.Verify(signature, hash, pubKey);
            }

            return res;


        }

        public byte[] Hash160(byte[] buffer)
        {
            byte[] sha256hash;
            using (SHA256 sHA256 = SHA256.Create())
            {
                sha256hash = sHA256.ComputeHash(buffer);
            }
            var ripemd160 = RIPEMD160Crypto.Create();

            var ripemd160hash = ripemd160.ComputeHash(sha256hash);
            return ripemd160hash;
        }
    }
}
