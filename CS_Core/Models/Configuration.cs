namespace CS_Core
{
    /// <summary>
    /// Configuration
    /// </summary>
    public sealed class Configuration
    {
        public CrawlerConfiguration? CrawlerConfiguration { get; set; }
        public HttpClientConfiguration? HttpClientConfiguration { get; set; }
    }
}
