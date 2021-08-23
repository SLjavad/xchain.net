using System.Text.Json.Serialization;

namespace XchainDotnet.Cosmos.Models.Tx
{
    /// <summary>
    /// Search Tx options
    /// </summary>
    public class SearchTxParams
    {
        [JsonPropertyName("messageAction")]
        public string MessageAction { get; set; }
        [JsonPropertyName("messageSender")]
        public string MessageSender { get; set; }
        [JsonPropertyName("transferSender")]
        public string TransferSender { get; set; }
        [JsonPropertyName("transferRecipient")]
        public string TransferRecipient { get; set; }
        [JsonPropertyName("page")]
        public int? Page { get; set; }
        [JsonPropertyName("limit")]
        public int? Limit { get; set; }
        [JsonPropertyName("txMinHeight")]
        public int? TxMinHeight { get; set; }
        [JsonPropertyName("txMaxHeight")]
        public int? TxMaxHeight { get; set; }
        [JsonPropertyName("rpcEndpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault | JsonIgnoreCondition.WhenWritingNull)]
        public string RpcEndpoint { get; set; }
    }
}
