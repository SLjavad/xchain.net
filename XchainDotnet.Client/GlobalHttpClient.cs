using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace XchainDotnet.Client
{
    public class GlobalHttpClient
    {
        private static HttpClient _httpClient;

        private GlobalHttpClient()
        {

        }

        public static HttpClient HttpClient
        {
            get
            {
                if (_httpClient == null)
                {
                    _httpClient = new ServiceCollection()
                        .AddHttpClient()
                        .BuildServiceProvider()
                        .GetService<IHttpClientFactory>()
                        .CreateClient();
                }
                return _httpClient;
            }
            set
            {
                //just for using in unit tests , but you can set your custom httpClient
                //it's better to use httpClientFactory client and don't set by yourself.
                _httpClient = value;
            }
        }
    }
}
