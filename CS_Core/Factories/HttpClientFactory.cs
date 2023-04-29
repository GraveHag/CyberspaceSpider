using System.Collections.Concurrent;
using System.Net;

namespace CS_Core
{
    /// <summary>
    /// HttpClientFactory
    /// </summary>
    public sealed class HttpClientFactory : IHttpClientFactory
    {
        readonly ConcurrentDictionary<string, HttpClient> httpClientDictionary;

        readonly HttpClientConfiguration configuration;

        public HttpClientFactory(Func<HttpClientConfiguration?> configuration)
        {
            httpClientDictionary = new ConcurrentDictionary<string, HttpClient>();

            this.configuration = configuration() ?? new HttpClientConfiguration();

        }

        public HttpClient CreateClient(string name)
        {
            if (!httpClientDictionary.TryGetValue(name, out HttpClient? httpClient))
            {
                httpClient = new HttpClient(GetHandler())
                {
                    Timeout = TimeSpan.FromSeconds(30)                    
                };
                httpClient.DefaultRequestHeaders.Add("User-Agent","CyberspaceSpiderUltimateProCrawlerUltra");
                httpClientDictionary[name] = httpClient;
            }

            return httpClient;
        }

        HttpClientHandler GetHandler() => new HttpClientHandler()
        {
            Proxy = configuration.UseProxy ? GetProxy() : null,
            AllowAutoRedirect = configuration.AllowAutoRedirect,
            UseProxy = configuration.UseProxy,
            
        };

        static WebProxy GetProxy()
        {
            Task<WebProxy> uriTask = Task.Run(async () => await ServiceCatalog.Mediate<IProxyService>().GetProxy().ConfigureAwait(false));
            
            uriTask.Wait();

            return uriTask.GetAwaiter().GetResult();
           
        }
    }
}
