using CS_Core;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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
            CancellationToken token = CancellationToken.None;

            await new SpiderMother((builder) =>
            {
                builder.blackList = LoadBlacklist();
                builder.configuration = LoadCrawlerConfiguration();

            }).Run(token);

        }
        static IFileService FileService() => ServiceCatalog.Mediate<IFileService>();

        static CrawlerConfiguration LoadCrawlerConfiguration()
        {
            IFileService fileService = FileService();

            string path = Path.Combine(AppContext.BaseDirectory,"../../..", Primitive.CrawlerConfiguration);

            if (!fileService.Exists(path)) throw new Exception($"No configuration file[{Primitive.CrawlerConfiguration}]");

            string configJson = fileService.LoadFileContent(path);

            if (string.IsNullOrEmpty(configJson)) throw new Exception("No parameters in configuration");

            return System.Text.Json.JsonSerializer.Deserialize<CrawlerConfiguration>(configJson) ?? new CrawlerConfiguration();

        }

        static string[]? LoadBlacklist()
        {
            IFileService fileService = FileService();

            string path = Path.Combine(AppContext.BaseDirectory, "../../..", Primitive.Blacklist);

            if (!fileService.Exists(path))
            {
                LogService.Info(nameof(Program), nameof(LoadBlacklist), "No black list provided");
                return default;
            }

            string blacklistContent = fileService.LoadFileContent(path);

            return blacklistContent.Split("\r", StringSplitOptions.RemoveEmptyEntries);

        }
    }
}