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
            CrawlerConfiguration config = new CrawlerConfiguration() {TimeToLive = new TimeSpan(0,0,1) };

            IWebCrawler crawler = config.CrawlerType switch
            {
                CrawlerType.CyberspaceSpider => WebCrawlerFactory.CreateCyberspaceSpider(() => config),
                CrawlerType.CyberspaceMiner => WebCrawlerFactory.CreateCyberspaceMiner(() => config),
                _ => throw new NotImplementedException(),
            };

            //await crawler.Run(CancellationToken.None);

            CancellationToken token = CancellationToken.None;

            CrawlerResponse? response = await crawler.GetUrl(new Uri("https://www.google.com"), token);
            Console.WriteLine(crawler.IsRunning ? $"{response?.CurrentDomain} {response?.StatusCode}" : "im death");
            
            response = await crawler.GetUrl(new Uri("https://www.facebook.com"), token);
            Console.WriteLine(crawler.IsRunning ? $"{response?.CurrentDomain} {response?.StatusCode}" : "im death");

            response = await crawler.GetUrl(new Uri("https://www.github.com"), token);
            Console.WriteLine(crawler.IsRunning ? $"{response?.CurrentDomain} {response?.StatusCode}" : "im death");
            
            response = await crawler.GetUrl(new Uri("https://www.seznam.cz"), token);
            Console.WriteLine(crawler.IsRunning ? $"{response?.CurrentDomain} {response?.StatusCode}" : "im death");
            
            response = await crawler.GetUrl(new Uri("https://www.twitter.com"), token);
            Console.WriteLine(crawler.IsRunning ? $"{response?.CurrentDomain} {response?.StatusCode}" : "im death");
            
            response = await crawler.GetUrl(new Uri("https://www.infoworld.com"), token);
            Console.WriteLine(crawler.IsRunning ? $"{response?.CurrentDomain} {response?.StatusCode}" : "im death");

            ;
        }

    }
}