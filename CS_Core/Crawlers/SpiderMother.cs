namespace CS_Core
{
    /// <summary>
    /// SpiderMother
    /// produces spiders based on provided type
    /// </summary>
    public sealed class SpiderMother
    {
        IEnumerable<Uri> visitedDomains = new List<Uri>();

        List<Uri> nextDomains = new List<Uri>();

        Uri startedDomain;

        readonly string[]? blackList;

        readonly CrawlerConfiguration configuration;

        public SpiderMother(SpiderMotherConfigurationBuilder builder)
        {
            blackList = builder.blackList;
            configuration = builder.configuration;
        }

        public SpiderMother(Action<SpiderMotherConfigurationBuilder> builder)
        {
            SpiderMotherConfigurationBuilder cfg = new SpiderMotherConfigurationBuilder();
            builder(cfg);

            blackList = cfg.blackList;
            configuration = cfg.configuration ?? throw new ArgumentNullException("Configuration is required!");
        }

        void PrepareRuns()
        {
            if (configuration.DomainsToCrawl.Count == 0) throw new InvalidOperationException("None domain to crawl");
            //todo zohlednit více started domains
            nextDomains.Add(configuration.DomainsToCrawl.AsEnumerable().ToUriList().First());

        }

        public async Task Run(CancellationToken token)
        {
            PrepareRuns();


           // TODO: naimplementovat více crawleru based on next uris ...
            IWebCrawler crawler = configuration!.CrawlerType switch
            {
                CrawlerType.CyberspaceSpider => CreateCyberspaceSpider(() => configuration),
                CrawlerType.CyberspaceMiner => CreateCyberspaceMiner(() => configuration),
                _ => throw new NotImplementedException(),
            };

            foreach (Uri uri in NextUri())
            {
                if (!crawler.IsRunning) break; //todo remove crawler
                CrawlerResponse response = await crawler.Crawl(uri, token);

                //todo 
                //nextDomains.AddRange(response.NextDomains);

            }

            LogService.Info(nameof(SpiderMother), nameof(Run), $"[{crawler.SpiderName}]: \"Out of websss..\"");
        }

        IEnumerable<Uri> NextUri()
        {
            foreach (Uri uri in nextDomains) yield return uri;
        }

        static IWebCrawler CreateCyberspaceSpider(Func<CrawlerConfiguration> configuration) => new CyberspaceSpider(configuration);
        static IWebCrawler CreateCyberspaceMiner(Func<CrawlerConfiguration> configuration) => throw new NotImplementedException();

    }
}
