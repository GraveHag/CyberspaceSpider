namespace CS_Core
{
    /// <summary>
    /// IConfigurationService
    /// </summary>
    public interface IConfigurationService
    {
        CrawlerConfiguration? CrawlerConfiguration { get; }

        HttpClientConfiguration? HttpClientConfiguration { get; }

        string ToString();
    }
}
