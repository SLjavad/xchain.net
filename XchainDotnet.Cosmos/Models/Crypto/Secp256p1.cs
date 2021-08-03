using Cryptography.ECDSA;
using Secp256k1Net;
using System;
using System.Linq;
using System.Security.Cryptography;
using XchainDotnet.Cosmos.Models.Crypto.RIPEMD160;

namespace XchainDotnet.Cosmos.Models.Crypto
{
    public class PrivateKeySecp256k1 : IPrivateKey
    {
        private readonly byte[] privKey;
        private PublicKeySecp256k1 pubKey;

        public PrivateKeySecp256k1(byte[] privKey)
        {
            pubKey = new PublicKeySecp256k1(Secp256K1Manager.GetPublicKey(privKey, true));
            this.privKey = privKey;
        }

        public IPublicKey GetPublicKey()
        {
            return pubKey;
        }

        public byte[] Sign(byte[] message)
        {
            var hash = SHA256.Create().ComputeHash(message);
            var signature = Secp256K1Manager.SignCompressedCompact(hash, privKey); //TODO: change lib // first field not needed
            signature = signature.Skip(1).ToArray();

            return signature;
        }

        public string ToBase64()
        {
            return Convert.ToBase64String(privKey);
        }

        public byte[] ToBuffer()
        {
            return privKey;
        }

        public string ToJsonInCodec()
        {
            return ToBase64();
        }

        public static IPrivateKey fromBase64(string value)
        {
            var buffer = Convert.FromBase64String(value);
            return new PrivateKeySecp256k1(buffer);
        }

        public static IPrivateKey FromJson(string value)
        {
            return fromBase64(value);
        }
    }

    public class PublicKeySecp256k1 : IPublicKey
    {
        private readonly byte[] pubKey;

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

        public string ToJsonInCodec()
        {
            return ToBase64();
        }

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
