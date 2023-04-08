using NLog.Fluent;
using System;
using System.Diagnostics;

namespace CS_Core
{
    /// <summary>
    /// Abstract webCrawler
    /// </summary>
    internal abstract class WebCrawler : IWebCrawler
    {
        readonly string _spiderName = NameGenerator.GetName();

        string IWebCrawler.SpiderName => _spiderName;

        protected TimeSpan LiveSpan = new TimeSpan(0, 0, 10);

        protected readonly Stopwatch _stopwatch;

        protected int MaxDepth;

        protected Uri StartedDomain = new Uri("https://www.google.com");

        protected bool IsAlive => LiveSpan.TotalMilliseconds > _stopwatch.Elapsed.TotalMilliseconds;

        protected HttpClient HttpClient => ServiceCatalog.Mediate<IHttpClientFactory>().CreateClient(_spiderName);

        bool IWebCrawler.IsRunning => IsAlive;

        protected async Task<HttpResponseMessage?> GetResponse(Uri uri, CancellationToken token)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);

                LogService.Info($"{GetType().FullName}:[{_spiderName}]", "GetUrl()", $"{uri}");

                return await HttpClient.SendAsync(request, token);
            }
            catch (Exception ex)
            {
                //todo error handling
                LogService.Fatal(ex, $"{GetType().FullName}:[{_spiderName}]", "GetResponse()");
                return null;
            }
        }

        public abstract Task Run(CancellationToken token);

        public abstract Task<CrawlerResponse?> GetUrl(Uri uri, CancellationToken token);

        public WebCrawler(Func<CrawlerConfiguration> configuration) : this()
        {
            LiveSpan = configuration().TimeToLive;
            MaxDepth = configuration().MaxDepth;
            StartedDomain = configuration().StartedDomain;
        }

        protected WebCrawler()
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
