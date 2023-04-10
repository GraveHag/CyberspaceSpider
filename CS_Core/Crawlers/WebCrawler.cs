using NLog.Fluent;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;

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

        protected TimeSpan TimeToRest = new TimeSpan(0, 0, 5);

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
            TimeToRest = configuration().TimeToRest;
            MaxDepth = configuration().MaxDepth;
        }

        int GetRetryDelay(RetryConditionHeaderValue? retryAfter)
        {
            int retryAfterDelay = TimeToRest.Seconds;

            if (retryAfter is not null && retryAfter.Delta.HasValue && (retryAfter.Delta.Value.Seconds > 0))
                retryAfterDelay = retryAfter.Delta.Value.Seconds;

            return retryAfterDelay;
        }


        protected async Task<HttpResponseMessage> GetResponse(Func<HttpRequestMessage> requestMessage, CancellationToken token)
        {
            try
            {
                HttpResponseMessage response = await HttpClient.SendAsync(requestMessage(), token);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.TooManyRequests:
                        {
                            Thread.Sleep(GetRetryDelay(response.Headers.RetryAfter) * 1000);
                            return await GetResponse(requestMessage, token);
                        }
                    case HttpStatusCode.Redirect:
                    case HttpStatusCode.RedirectKeepVerb:
                    case HttpStatusCode.RedirectMethod:
                        {
                            if (response.Headers.Location is null) throw new ArgumentException(nameof(response.Headers.Location));
                            return await GetResponse(() => new HttpRequestMessage(HttpMethod.Get, response.Headers.Location.AbsoluteUri), token);
                        }
                };

                return response;
            }
            catch (Exception ex)
            {
                LogService.Fatal(ex, $"{GetType().FullName}:[{_spiderName}]", nameof(GetResponse));
                return new HttpResponseMessage() { StatusCode = HttpStatusCode.ServiceUnavailable };
            }
        }

        public abstract Task<CrawlerResponse> Crawl(Uri uri, CancellationToken token);

        protected async Task<CrawlerResponse> ReadHtmlContentAsync(string htmlContent)
        {
            IHtmlParserService htmlParserService = ServiceCatalog.Mediate<IHtmlParserService>();

            CrawlerResponse response = new CrawlerResponse()
            {
                NextDomains = await htmlParserService.ExtractDomainsFromContentAsync(htmlContent),
                MetaTags = await htmlParserService.ExtractMetaTagsFromContentAsync(htmlContent)
            };

            return response;
        }
    }
}
