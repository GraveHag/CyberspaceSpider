namespace CS_Core
{
    /// <summary>
    /// WebCrawlerFactory
    /// produces crawlers based on provided type
    /// </summary>
    public static class WebCrawlerFactory
    {
        public static IWebCrawler CreateCyberspaceSpider(Func<CrawlerConfiguration> configuration) => new CyberspaceSpider(configuration);
        public static IWebCrawler CreateCyberspaceMiner(Func<CrawlerConfiguration> configuration) => throw new NotImplementedException();

    }
}
