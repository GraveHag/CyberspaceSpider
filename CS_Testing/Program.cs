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
            CrawlerConfiguration config = new CrawlerConfiguration() { TimeToLive = new TimeSpan(0, 0, 1) };

            IWebCrawler crawler = config.CrawlerType switch
            {
                CrawlerType.CyberspaceSpider => WebCrawlerFactory.CreateCyberspaceSpider(() => config),
                CrawlerType.CyberspaceMiner => WebCrawlerFactory.CreateCyberspaceMiner(() => config),
                _ => throw new NotImplementedException(),
            };

            //await crawler.Run(CancellationToken.None);

            CancellationToken token = CancellationToken.None;

            foreach (Uri uri in NextUri())
            {
                if (!crawler.IsRunning) break;
                CrawlerResponse? response = await crawler.GetUrl(uri, token);
                Console.WriteLine($"{response?.CurrentDomain} {response?.StatusCode}");
            }
            
            Console.WriteLine("Im Death");
        }

        static IEnumerable<Uri> NextUri()
        {
            IList<Uri> urls = new List<Uri>() {
                new Uri("https://www.google.com"),
                new Uri("https://www.github.com"),
                new Uri("https://www.seznam.cz"),
                new Uri("https://www.twitter.com"),
                new Uri("https://www.infoworld.com")
            };

            foreach (Uri uri in urls) yield return uri;
            
        }


    }
}