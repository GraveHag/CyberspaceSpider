namespace CS_Core
{
    /// <summary>
    /// CyberspaceSpider
    /// type of web crawler
    /// </summary>
    internal sealed class CyberspaceSpider : WebCrawler
    {
        public CyberspaceSpider(Func<CrawlerConfiguration> configuration) : base(configuration)
        {
        }

        CyberspaceSpider() { }

        public override async Task<CrawlerResponse> Crawl(Uri uri, CancellationToken token)
        {
            using HttpResponseMessage response = await GetResponse(() => new HttpRequestMessage(HttpMethod.Get, uri), token);

            LogService.Info($"{GetType().Name}:[{_spiderName}]", nameof(Crawl), $"{uri} [{response.StatusCode}]");

            if (!response.IsSuccessStatusCode) 
                return new CrawlerResponse { CurrentDomain = uri, StatusCode = response.StatusCode };

            string htmlContent = await response.Content.ReadAsStringAsync(token);

            CrawlerResponse crawlerResponse = await ReadHtmlContentAsync(htmlContent);
            crawlerResponse.CurrentDomain = uri;

            Thread.Sleep(TimeToRest);

            return crawlerResponse;

            
        }
    }
}
