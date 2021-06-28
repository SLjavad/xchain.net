using Cryptography.ECDSA;
using Secp256k1Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xchain.net.xchain.cosmos.Models.Crypto.RIPEMD160;

namespace Xchain.net.xchain.cosmos.Models.Crypto
{
    public class PrivateKeySecp256k1 : IPrivateKey
    {
        private readonly byte[] privKey;
        private PublicKeySecp256k1 pubKey;

        public PrivateKeySecp256k1(byte[] privKey)
        {
            this.pubKey = new PublicKeySecp256k1(Secp256K1Manager.GetPublicKey(privKey, true));
            this.privKey = privKey;
        }

        public IPublicKey GetPublicKey()
        {
            return this.pubKey;
        }

        public byte[] Sign(byte[] message)
        {
            var hash = SHA256.Create().ComputeHash(message);
            var signature = Secp256K1Manager.SignCompressedCompact(hash, this.privKey); //TODO: change lib

            return signature;
        }

        public string ToBase64()
        {
            return Convert.ToBase64String(this.privKey);
        }

        public byte[] ToBuffer()
        {
            return this.privKey;
        }

        public string ToJsonInCodec()
        {
            return this.ToBase64();
        }

        public static IPrivateKey fromBase64(string value)
        {
            var buffer = Convert.FromBase64String(value);
            return new PrivateKeySecp256k1(buffer);
        }

        public static IPrivateKey FromJson(string value)
        {
            return PrivateKeySecp256k1.fromBase64(value);
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
            return this.Hash160(this.pubKey);
        }

        public string ToBase64()
        {
            return Convert.ToBase64String(this.pubKey);
        }

        public byte[] ToBuffer()
        {
            return this.pubKey;
        }
        
        public string toJsonInCodec()
        {
            return this.ToBase64();
        }

        public IPublicKey fromBase64(string value)
        {
            return new PublicKeySecp256k1(Convert.FromBase64String(value));
        }

        public IPublicKey fromJSON(string value)
        {
            return fromBase64(value);
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
                res = secp256K1.Verify(signature, hash, this.pubKey);
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
