using System.Text;
using System.Text.Json;

namespace CS_Core
{
    /// <summary>
    /// ConfigurationService
    /// </summary>
    public sealed class ConfigurationService : IConfigurationService
    {
        readonly CrawlerConfiguration? CrawlerConfiguration;
        readonly HttpClientConfiguration? HttpClientConfiguration;

        static IFileService FileService() => ServiceCatalog.Mediate<IFileService>();

        string AppPath { get; set; } = Path.Combine(AppContext.BaseDirectory, "../../..");

        public ConfigurationService()
        {
            Configuration config = LoadConfiguration();

            CrawlerConfiguration = config.CrawlerConfiguration;
            HttpClientConfiguration = config.HttpClientConfiguration;

            if (CrawlerConfiguration is null) return;
            CrawlerConfiguration.Blacklist = LoadBlacklist();
        }

        public ConfigurationService(string path) : base()
        {
            AppPath = path;
        }

        CrawlerConfiguration? IConfigurationService.CrawlerConfiguration => CrawlerConfiguration;
        HttpClientConfiguration? IConfigurationService.HttpClientConfiguration => HttpClientConfiguration;

        Configuration LoadConfiguration()
        {
            IFileService fileService = FileService();

            string path = Path.Combine(AppPath, Primitive.ConfigurationJson);

            if (!fileService.Exists(path)) throw new Exception($"No configuration file[{Primitive.ConfigurationJson}]");

            string configJson = fileService.LoadFileContent(path);

            if (string.IsNullOrEmpty(configJson)) throw new Exception("No parameters in configuration");

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Converters = { new TimeSpanConverter() }
            };

            return JsonSerializer.Deserialize<Configuration>(configJson, options) ?? throw new ArgumentNullException();

        }

        string[]? LoadBlacklist()
        {
            IFileService fileService = FileService();

            string path = Path.Combine(AppPath, Primitive.Blacklist);

            if (!fileService.Exists(path))
            {
                LogService.Info(nameof(ConfigurationService), nameof(LoadBlacklist), "No black list provided");
                return default;
            }

            string blacklistContent = fileService.LoadFileContent(path);

            return blacklistContent.Split("\r", StringSplitOptions.RemoveEmptyEntries);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            char hash = '.';

            sb.AppendLine();
            sb.Append(hash, 60);
            sb.AppendLine();
            sb.AppendLine($"  {nameof(CrawlerConfiguration)}:");

            sb.AppendLine($"\t {nameof(CrawlerConfiguration.CrawlerType)}: [{CrawlerConfiguration?.CrawlerType}]");
            sb.AppendLine($"\t {nameof(CrawlerConfiguration.TimeToLive)}: [{CrawlerConfiguration?.TimeToLive}]");
            sb.AppendLine($"\t {nameof(CrawlerConfiguration.TimeToRest)}: [{CrawlerConfiguration?.TimeToRest}]");
            sb.AppendLine($"\t {nameof(CrawlerConfiguration.MaxDepth)}: [{CrawlerConfiguration?.MaxDepth}]");
            sb.AppendLine($"\t {nameof(CrawlerConfiguration.MaxSpiders)}: [{CrawlerConfiguration?.MaxSpiders}]");
            sb.AppendLine($"\t {nameof(CrawlerConfiguration.DomainsToCrawl)}: [{CrawlerConfiguration?.DomainsToCrawl.Length}]");
            sb.AppendLine($"\t {nameof(CrawlerConfiguration.Blacklist)}: [{CrawlerConfiguration?.Blacklist?.Length > 0}]");

            sb.AppendLine($"  {nameof(HttpClientConfiguration)}:");

            sb.AppendLine($"\t {nameof(HttpClientConfiguration.AllowAutoRedirect)}: [{HttpClientConfiguration?.AllowAutoRedirect}]");
            sb.AppendLine($"\t {nameof(HttpClientConfiguration.HttpTimeout)}: [{HttpClientConfiguration?.HttpTimeout}]");
            sb.AppendLine($"\t {nameof(HttpClientConfiguration.UseProxy)}: [{HttpClientConfiguration?.UseProxy}]");

            sb.Append(hash, 60);

            return sb.ToString();
        }

    }
}
