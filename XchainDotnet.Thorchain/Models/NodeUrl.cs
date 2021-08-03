using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchain.net.xchain.thorchain.Models
{
    public class NodeUrl
    {
        public NodeUrl(string node, string rPC)
        {
            Node = node;
            RPC = rPC;
        }

        public NodeUrl()
        {

        }

        public string Node { get; set; }
        public string RPC { get; set; }
    }
}
