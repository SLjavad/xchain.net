namespace XchainDotnet.Thorchain.Models
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
