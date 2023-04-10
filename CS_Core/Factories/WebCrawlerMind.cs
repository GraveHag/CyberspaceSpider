
namespace CS_Core
{

    public class CrawlerconfigurationBuilder
    {
        public CrawlerConfiguration configuration;
        public IList<Uri> blackList;

        public CrawlerconfigurationBuilder(CrawlerConfiguration configuration, IList<Uri> blackList)
        {
            this.configuration = configuration;
            this.blackList = blackList;
        }

        public CrawlerconfigurationBuilder(CrawlerConfiguration configuration)
        {
            this.configuration = configuration;
            blackList = new List<Uri>();
        }
    }


    /// <summary>
    /// WebCrawlerFactory
    /// produces crawlers based on provided type
    /// </summary>
    public sealed class WebCrawlerMind
    {
        IEnumerable<Uri> domains = new List<Uri>();

        IList<Uri> blackList = new List<Uri>();

        readonly CrawlerConfiguration configuration;

        public WebCrawlerMind(Func<CrawlerconfigurationBuilder> builder)
        {
            configuration = builder().configuration;
            blackList = builder().blackList;
        }

        public WebCrawlerMind StoreBlackList(IList<string> blackList) { 
            
            return this; 
        }

        public async Task Run(CancellationToken token)
        {
            IWebCrawler crawler = configuration.CrawlerType switch
            {
                CrawlerType.CyberspaceSpider => CreateCyberspaceSpider(() => configuration),
                CrawlerType.CyberspaceMiner => CreateCyberspaceMiner(() => configuration),
                _ => throw new NotImplementedException(),
            };

            foreach (Uri uri in NextUri())
            {
                if (!crawler.IsRunning) break;
                CrawlerResponse? response = await crawler.Crawl(uri, token);
            }

            LogService.Info(nameof(WebCrawlerMind), nameof(Run), $"[{crawler.SpiderName}]: \"Out of websss..\"");
        }

        IEnumerable<Uri> NextUri()
        {
            foreach (Uri uri in domains) yield return uri;
        }

        static IWebCrawler CreateCyberspaceSpider(Func<CrawlerConfiguration> configuration) => new CyberspaceSpider(configuration);
        static IWebCrawler CreateCyberspaceMiner(Func<CrawlerConfiguration> configuration) => throw new NotImplementedException();

    }
}
