namespace CS_Core
{
    /// <summary>
    /// Crawler configuration
    /// </summary>
    public sealed class CrawlerConfiguration
    {
        public TimeSpan TimeToLive { get; set; } = new TimeSpan(0, 0, 10);

        public CrawlerType CrawlerType { get; set; }

        public int MaxDepth { get; set; } = 5;

        public Uri StartedDomain { get; set; } = new Uri("https://www.github.com");
    }
}
