using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchain.net.xchain.cosmos.Models.Crypto;
using Xchain.net.xchain.cosmos.Models.Tx;
using Xchain.net.xchain.cosmos.SDK;

namespace Xchain.net.xchain.cosmos
{
    public class Auth
    {
        public static StdTx SignStdTx(CosmosSdkClient cosmosSdkClient, IPrivateKey privateKey, StdTx stdTx, string accountNumber, string sequence)
        {
            var signBytes = stdTx.GetSignBytes(cosmosSdkClient.chainId, accountNumber, sequence);

            var sign = new StdSignature
            {
                PubKey = privateKey.GetPublicKey(),
                Signature = Encoding.UTF8.GetString(privateKey.Sign(signBytes))
            };

            //TODO: WIP newStdTx
        }
    }
}
