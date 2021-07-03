using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Xchain.net.xchain.client
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
