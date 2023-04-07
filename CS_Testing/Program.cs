using CS_Core;

namespace CS_Testing
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ServiceCatalog.RegisterAllService();

            await Run();
        }

        static async Task Run()
        {
            //set by provided configuration
            CrawlerConfiguration config = new CrawlerConfiguration();

            IWebCrawler crawler = config.CrawlerType switch
            {
                CrawlerType.CyberspaceSpider => WebCrawlerFactory.CreateCyberspaceSpider(() => config),
                CrawlerType.CyberspaceMiner => WebCrawlerFactory.CreateCyberspaceMiner(() => config),
                _ => throw new NotImplementedException(),
            };

            //await crawler.Run(CancellationToken.None);

            CrawlerResponse? response = await crawler.GetUrl(new Uri("https://www.google.com"), CancellationToken.None);

            ;
        }

    }
}