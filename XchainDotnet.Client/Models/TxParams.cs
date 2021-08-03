namespace XchainDotnet.Client.Models
{
    public class TxParams
    {
        public Asset Asset { get; set; }
        public decimal Amount { get; set; }
        public string Recipient { get; set; }
        public string Memo { get; set; }
    }
}
