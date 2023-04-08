using System.Net;

namespace CS_Core
{
    /// <summary>
    /// HttpClientFactory
    /// </summary>
    public sealed class HttpClientFactory : IHttpClientFactory, IService
    {
        readonly IDictionary<string, HttpClient> httpClientDictionary;

        public HttpClientFactory()
        {
            httpClientDictionary = new Dictionary<string, HttpClient>();
        }

        public HttpClient CreateClient(string name)
        {
            if (!httpClientDictionary.TryGetValue(name, out HttpClient? httpClient))
            {
                httpClient = new HttpClient(GetHandler())
                {
                    Timeout = TimeSpan.FromSeconds(10)                    
                };

                httpClientDictionary[name] = httpClient;
            }

            return httpClient;

        }

        HttpClientHandler GetHandler() => new HttpClientHandler()
        {
            AllowAutoRedirect = false,
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };

    }
}
