namespace CS_Core
{
    /// <summary>
    /// Web crawler interface
    /// </summary>
    public interface IWebCrawler
    {
        Task<CrawlerResponse?> Crawl(Uri uri, CancellationToken token);

        bool IsRunning { get; }

        string SpiderName { get; }
    }
}
