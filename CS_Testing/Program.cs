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

            LogService.Info(nameof(Program), nameof(Main), $"Start - {DateTime.Now}");

            ServiceCatalog.RegisterAllService();

            IConfigurationService configuration = new ConfigurationService();

            ServiceCatalog.RegisterService<IConfigurationService>(configuration);

            ServiceCatalog.RegisterService<IHttpClientFactory>(new HttpClientFactory(() =>
            {
                return configuration.HttpClientConfiguration;

            }));

            LogService.Info(nameof(Program),nameof(Main), configuration.ToString());

            await Run();

            LogService.Info(nameof(Program), nameof(Main), $"Stop - {DateTime.Now} [{Stopwatch.Elapsed.TotalSeconds} sec]");

        }

        static async Task Run()
        {
            CancellationToken token = CancellationToken.None;
            IConfigurationService configuration = ServiceCatalog.Mediate<IConfigurationService>();

            await new SpiderMother(configuration.CrawlerConfiguration).Run(token);
        }
    }
}