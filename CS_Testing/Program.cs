using CS_Core;
using System.Net;

namespace CS_Testing
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            LogService.Info("Program", "Test", "testík");

            ConfigureServices.Configure();
            ConfigureServices.ConfigureHttpClientFactory(() =>
            {
                return new HttpClientConfiguration
                {
                    AllowAutoRedirect = true,
                    Timeout = TimeSpan.FromSeconds(15),
                    UseProxy = false
                };
            });

            await Run();
        }

        static async Task Run()
        {
            //set by provided configuration
            CrawlerConfiguration config = new CrawlerConfiguration() { TimeToLive = new TimeSpan(0, 5, 0) };
            
            CancellationToken token = CancellationToken.None;

            ConfigureServices.ConfigureHttpClientFactory(() =>
            {
                return new HttpClientConfiguration()
                {
                    AllowAutoRedirect = true,
                    Timeout = TimeSpan.FromSeconds(15),
                    UseProxy = true
                };
            });


            IWebCrawler crawler = config.CrawlerType switch
            {
                CrawlerType.CyberspaceSpider => WebCrawlerFactory.CreateCyberspaceSpider(() => config),
                CrawlerType.CyberspaceMiner => WebCrawlerFactory.CreateCyberspaceMiner(() => config),
                _ => throw new NotImplementedException(),
            };

            foreach (Uri uri in NextUri())
            {
                if (!crawler.IsRunning) break;
                CrawlerResponse? response = await crawler.GetUrl(uri, token);
                Console.WriteLine(response is null ? "---" : $"{response?.CurrentDomain} {response?.StatusCode}");
            }
            
            Console.WriteLine($"[{crawler.SpiderName}]: \"Out of websss..\"");
        }

        static IEnumerable<Uri> NextUri()
        {
            IList<Uri> urls = new List<Uri>() {
                new Uri("https://www.github.com"),
                new Uri("https://www.google.com"),
                new Uri("https://www.seznam.cz"),
                new Uri("https://www.twitter.com"),
                new Uri("https://www.infoworld.com")
            };

            foreach (Uri uri in urls) yield return uri;
            
        }
    }
}