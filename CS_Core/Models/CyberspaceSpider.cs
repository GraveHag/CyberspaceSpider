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

        public CyberspaceSpider() { }

        public override async Task<CrawlerResponse?> Crawl(Uri uri, CancellationToken token)
        {
            if (!IsAlive) return default;

            HttpResponseMessage response = await GetResponse(uri, token);

            LogService.Info($"{GetType().Name}:[{_spiderName}]", nameof(Crawl), $"{uri} [{response.StatusCode}]");

            if (!response.IsSuccessStatusCode) return new CrawlerResponse { CurrentDomain = uri.ToString(), StatusCode = (int)response.StatusCode };

            string htmlContent = await response.Content.ReadAsStringAsync(token);

            CrawlerResponse crawlerResponse = new CrawlerResponse { CurrentDomain = uri.ToString() };

            //todo
            ReadHtmlContent(htmlContent);

            return crawlerResponse;

        }
    }
}
