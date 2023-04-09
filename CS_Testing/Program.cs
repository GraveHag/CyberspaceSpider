using CS_Core;
using System.Diagnostics;
using System.Net;

namespace CS_Testing
{
    internal class Program
    {
        static Stopwatch Stopwatch = new Stopwatch();

        static async Task Main(string[] args)
        {
            Stopwatch.Start();

            LogService.Info("Program", nameof(Main), $"Start - {DateTime.Now}");

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

            LogService.Info("Program", nameof(Main), $"Stop - {DateTime.Now} [{Stopwatch.Elapsed.TotalSeconds} sec]");

        }

        static async Task Run()
        {

            CrawlerConfiguration config = new CrawlerConfiguration() { TimeToLive = new TimeSpan(0, 5, 0) };
            CancellationToken token = CancellationToken.None;

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
                Console.WriteLine(response is null ? "---" : $"{response?.CurrentDomain} {response?.StatusCode}, next domains: {response?.NextDomains?.Count()}");
            }

            LogService.Info(nameof(Program), nameof(Run), $"[{crawler.SpiderName}]: \"Out of websss..\"");
        }

        static IEnumerable<Uri> NextUri()
        {
            IList<Uri> urls = new List<Uri>() {
                new Uri("https://www.seznam.cz"),
                new Uri("https://www.facebook.com"),
                new Uri("https://www.instagram.com"),
                new Uri("https://www.twitter.com"),
                new Uri("https://www.linkedin.com"),
                new Uri("https://www.reddit.com"),
                new Uri("https://www.netflix.com"),
                new Uri("https://www.amazon.com"),
                new Uri("https://www.wikipedia.org"),
                new Uri("https://www.apple.com"),
                new Uri("https://www.microsoft.com"),
                new Uri("https://www.yahoo.com"),
                new Uri("https://www.dropbox.com"),
                new Uri("https://www.github.com"),
                new Uri("https://www.wordpress.com"),
                new Uri("https://www.bing.com"),
                new Uri("https://www.paypal.com"),
                new Uri("https://www.imdb.com"),
                new Uri("https://www.spotify.com"),
                new Uri("https://www.stackoverflow.com"),
                new Uri("https://www.tumblr.com"),
                new Uri("https://www.pinterest.com"),
                new Uri("https://www.aliexpress.com"),
                new Uri("https://www.booking.com"),
                new Uri("https://www.cnn.com"),
                new Uri("https://www.nytimes.com"),
                new Uri("https://www.bbc.com"),
                new Uri("https://www.aljazeera.com")
            };

            foreach (Uri uri in urls) yield return uri;

        }
    }
}