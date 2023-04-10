namespace CS_Core
{
    /// <summary>
    /// SpiderMotherConfigurationBuilder
    /// to configure spider mother
    /// </summary>
    public sealed class SpiderMotherConfigurationBuilder
    {
        public CrawlerConfiguration Configuration { get; set; } = new CrawlerConfiguration();

        public string[]? BlackList { get; set; }
    }
}
