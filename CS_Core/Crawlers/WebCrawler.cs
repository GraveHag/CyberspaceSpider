using System.Diagnostics;

namespace CS_Core
{
    /// <summary>
    /// Abstract webCrawler
    /// </summary>
    internal abstract class WebCrawler : IWebCrawler
    {
        protected readonly string _spiderName = NameGenerator.GetName();

        string IWebCrawler.SpiderName => _spiderName;

        protected TimeSpan LiveSpan = new TimeSpan(0, 0, 10);

        protected readonly Stopwatch _stopwatch;

        protected int MaxDepth;

        protected bool IsAlive => LiveSpan.TotalMilliseconds > _stopwatch.Elapsed.TotalMilliseconds;

        protected HttpClient HttpClient => ServiceCatalog.Mediate<IHttpClientFactory>().CreateClient(_spiderName);

        bool IWebCrawler.IsRunning => IsAlive;

        protected WebCrawler()
        {
            _stopwatch = new Stopwatch();

            _stopwatch.Start();
        }

        public WebCrawler(Func<CrawlerConfiguration> configuration) : this()
        {
            LiveSpan = configuration().TimeToLive;
            MaxDepth = configuration().MaxDepth;
        }

        protected async Task<HttpResponseMessage> GetResponse(Uri uri, CancellationToken token)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);
                HttpResponseMessage response = await HttpClient.GetAsync(uri, token);
                //todo error handling
                //use another proxy if server refuse
                return response;
            }
            catch (Exception ex)
            {
                LogService.Fatal(ex, $"{GetType().FullName}:[{_spiderName}]", nameof(GetResponse));
                return new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.ServiceUnavailable };
            }
        }

        public abstract Task<CrawlerResponse> Crawl(Uri uri, CancellationToken token);

        protected async Task<CrawlerResponse> ReadHtmlContentAsync(string htmlContent)
        {
            IHtmlParserService htmlParserService = ServiceCatalog.Mediate<IHtmlParserService>();

            CrawlerResponse response = new CrawlerResponse()
            {
                NextDomains = await htmlParserService.ExtractDomainsFromContentAsync(htmlContent),
            };

            //var test = await htmlParserService.ExtractMetaTagsFromContentAsync(htmlContent);

            return response;
        }
    }
}
