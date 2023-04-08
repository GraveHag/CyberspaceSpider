namespace CS_Core
{
    /// <summary>
    /// Web crawler interface
    /// </summary>
    public interface IWebCrawler
    {
        Task<CrawlerResponse?> GetUrl(Uri uri, CancellationToken token);

        Task Run(CancellationToken token);

        bool IsRunning { get; }

        string SpiderName { get; }
    }
}
