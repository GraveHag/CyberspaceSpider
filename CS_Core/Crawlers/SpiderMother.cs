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

        int failedEncounter = 0;

        readonly CrawlerConfiguration configuration;

        public SpiderMother(CrawlerConfiguration? configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException("Configuration is required!");
        }

        void PrepareRuns()
        {
            if (configuration.DomainsToCrawl.Length == 0) throw new InvalidOperationException("None domain to crawl");
            
            //todo zohlednit více started domains
            nextDomains.AddRange(configuration.DomainsToCrawl.AsEnumerable().ToUriList());
        }

        bool Valid(Uri uri)
        {
            bool conflict = configuration.Blacklist?.Where(p => p.Contains(uri.OriginalString)).Any() ?? false;
            
            if (conflict) {
                LogService.Warn(nameof(SpiderMother), nameof(Valid), $"{uri.OriginalString} is on blacklist");

                failedEncounter++;

                return false;
            }

            return true;
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
                if (!Valid(uri)) continue;
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
