using System;
using System.Diagnostics;

namespace CS_Core
{
    /// <summary>
    /// Abstract webCrawler
    /// </summary>
    public abstract class WebCrawler : IWebCrawler
    {
        protected string Name => GetType().Name;

        protected TimeSpan LiveSpan = new TimeSpan(0, 0, 10);

        protected readonly Stopwatch _stopwatch;

        protected int MaxDepth;

        protected Uri StartedDomain = new Uri("https://www.google.com");

        protected bool IsAlive => LiveSpan.TotalMilliseconds > _stopwatch.Elapsed.TotalMilliseconds;

        protected static HttpClient HttpClient => ServiceCatalog.Mediate<IHttpClientFactory>().CreateClient();
        
        protected async Task<HttpResponseMessage> GetResponse(Uri uri, CancellationToken token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);

            LogService.Info(Name, "GetUrl()", $"{uri}");

            return await HttpClient.SendAsync(request, token);
        }

        public abstract Task Run(CancellationToken token);

        public abstract Task<CrawlerResponse?> GetUrl(Uri uri, CancellationToken token);

        public WebCrawler(Func<CrawlerConfiguration> configuration) : this()
        {
            LiveSpan = configuration().TimeToLive;
            MaxDepth = configuration().MaxDepth;
            StartedDomain = configuration().StartedDomain;
        }

        public WebCrawler()
        {
            _stopwatch = new Stopwatch();

            _stopwatch.Start();

        }

        protected void ReadHtmlContent(string htmlContent)
        {
            //todo HtmlAgilityPack
        }
    }
}
