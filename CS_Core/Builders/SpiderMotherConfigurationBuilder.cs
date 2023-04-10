namespace CS_Core
{
    /// <summary>
    /// SpiderMotherConfigurationBuilder
    /// to configure spider mother
    /// </summary>
    public sealed class SpiderMotherConfigurationBuilder
    {
        public CrawlerConfiguration configuration { get; set; } = new CrawlerConfiguration();

        public string[]? blackList;
    }
}
