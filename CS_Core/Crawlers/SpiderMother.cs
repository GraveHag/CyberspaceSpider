using AngleSharp;
using AngleSharp.Dom;
using System;

namespace CS_Core
{
    /// <summary>
    /// SpiderMother
    /// produces spiders based on provided type
    /// </summary>
    public sealed class SpiderMother
    {
        List<Uri> visitedDomains = new List<Uri>();

        List<Uri> nextDomains = new List<Uri>();

        int maxDomainsToVisit = 100;

        int failedEncounter = 0;

        readonly CrawlerConfiguration configuration;

        List<IWebCrawler> crawlers = new List<IWebCrawler>();

        public List<Uri> Results => visitedDomains;

        public SpiderMother(CrawlerConfiguration? configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException("Configuration is required!");
        }

        IWebCrawler CreateSpider() => configuration!.CrawlerType switch
        {
            CrawlerType.CyberspaceSpider => CreateCyberspaceSpider(() => configuration),
            CrawlerType.CyberspaceMiner => CreateCyberspaceMiner(() => configuration),
            _ => throw new NotImplementedException(),
        };

        void TestLifespan()
        {
            for (int i = 0; i < crawlers.Count; i++)
            {
                if (!crawlers[i].IsAlive)
                {
                    LogService.Info(nameof(SpiderMother), nameof(TestLifespan), $"{crawlers[i].SpiderName}: out of webssss...");
                    crawlers[i] = CreateSpider();
                }
            }

        }

        void PrepareRuns()
        {
            if (string.IsNullOrEmpty(configuration.StartedDomain)) throw new InvalidOperationException("None domain to crawl");
            nextDomains.Add(new Uri(configuration.StartedDomain));

            for (int i = 0; i < configuration.MaxSpiders; i++)
                crawlers.Add(CreateSpider());

        }

        bool Valid(Uri uri)
        {
            bool conflict = configuration.Blacklist?.Where(p => p.Contains(uri.OriginalString)).Any() ?? false;

            if (conflict)
            {
                LogService.Warn(nameof(SpiderMother), nameof(Valid), $"{uri.OriginalString} is on blacklist");

                failedEncounter++;

                return false;
            }

            return true;
        }

        public async Task Run(CancellationToken token)
        {
            PrepareRuns();

            do
            {
                List<Uri> nextUris = new List<Uri>(); //to be handle in future
                List<Uri> currentUris = nextDomains; //current 
                
                while (true)
                {
                    List<Task<CrawlerResponse>> crawlersResponse = new List<Task<CrawlerResponse>>();

                    int counter = 0;

                    foreach (Uri uri in currentUris.Take(configuration.MaxSpiders))
                    {
                        if (!Valid(uri))
                        {
                            visitedDomains.Add(uri);
                            continue;
                        };

                        crawlersResponse.Add(crawlers[counter].Crawl(uri, token));

                        counter = crawlersResponse.Count;
                    }

                    if (crawlersResponse.Count == 0 || visitedDomains.Count > maxDomainsToVisit) break;

                    CrawlerResponse[] results = await Task.WhenAll(crawlersResponse);

                    foreach (CrawlerResponse response in results)
                    {
                        if (response.CurrentDomain is null) continue;
                        visitedDomains.Add(response.CurrentDomain);

                        if (response.NextDomains is null) continue;
                        nextUris.AddRange(response.NextDomains.Where(domain => !visitedDomains.Contains(domain)));

                    }

                    nextDomains = nextUris;
                    currentUris.RemoveRange(0, counter);

                    TestLifespan();
                }

                LogService.Info(nameof(SpiderMother), nameof(Run), $"Next round - visited[{visitedDomains.Count()}]");

            } while (visitedDomains.Count < maxDomainsToVisit);

        }

        static IWebCrawler CreateCyberspaceSpider(Func<CrawlerConfiguration> configuration) => new CyberspaceSpider(configuration);
        static IWebCrawler CreateCyberspaceMiner(Func<CrawlerConfiguration> configuration) => throw new NotImplementedException();

    }
}
