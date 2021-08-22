namespace XchainDotnet.Thorchain.Models
{
    /// <summary>
    /// Node URL object
    /// </summary>
    public class NodeUrl
    {
        /// <summary>
        /// Node URL object
        /// </summary>
        /// <param name="node">node address</param>
        /// <param name="rPC">RPC address</param>
        public NodeUrl(string node, string rPC)
        {
            Node = node;
            RPC = rPC;
        }

        public NodeUrl()
        {

        }

        /// <summary>
        /// Node address
        /// </summary>
        public string Node { get; set; }
        /// <summary>
        /// RPC address
        /// </summary>
        public string RPC { get; set; }
    }
}
