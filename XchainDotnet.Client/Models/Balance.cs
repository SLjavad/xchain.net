namespace XchainDotnet.Client.Models
{
    public class Balance
    {
        /// <summary>
        /// Asset of the balance
        /// </summary>
        public Asset Asset { get; set; }
        /// <summary>
        /// balance amount
        /// </summary>
        public decimal Amount { get; set; }
    }
}
