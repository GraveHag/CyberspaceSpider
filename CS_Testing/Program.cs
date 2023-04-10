using CS_Core;
using System.Diagnostics;

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

            await new WebCrawlerMind(() => new CrawlerconfigurationBuilder(config)).StoreBlackList(new List<string>()).Run(CancellationToken.None);

        }

    }
}