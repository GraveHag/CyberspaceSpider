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

        public override async Task<CrawlerResponse?> GetUrl(Uri uri, CancellationToken token)
        {
            if (!IsAlive) return default;

            HttpResponseMessage response = await GetResponse(uri, token);
            if (!response.IsSuccessStatusCode) return new CrawlerResponse { CurrentDomain = uri.ToString(), StatusCode = (int)response.StatusCode };

            string htmlContent = await response.Content.ReadAsStringAsync(token);

            CrawlerResponse crawlerResponse = new CrawlerResponse { CurrentDomain = uri.ToString() };

            //todo
            ReadHtmlContent(htmlContent);

            return crawlerResponse;

        }

        public override async Task Run(CancellationToken token)
        {
            //todo another possibilities 
            while (IsAlive)
            {
                await GetUrl(StartedDomain, token);
            }

        }
    }
}
