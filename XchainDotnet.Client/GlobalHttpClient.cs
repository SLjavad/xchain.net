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
        }
    }
}
