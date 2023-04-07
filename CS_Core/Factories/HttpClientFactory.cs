using System.Net;

namespace CS_Core
{
    /// <summary>
    /// HttpClientFactory
    /// </summary>
    public sealed class HttpClientFactory : IHttpClientFactory, IService
    {
        public HttpClientFactory()
        {
        }

        public HttpClient CreateClient(string name)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,                
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            return new HttpClient(httpClientHandler)
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
        }
    }
}
