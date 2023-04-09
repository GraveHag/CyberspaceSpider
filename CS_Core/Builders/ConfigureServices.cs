
namespace CS_Core
{
    /// <summary>
    /// ConfigureServices
    /// into DI container
    /// </summary>
    public static class ConfigureServices
    {
        public static void Configure()
        {
            ServiceCatalog.RegisterAllService();

        }

        public static void ConfigureHttpClientFactory(Func<HttpClientConfiguration> config) => 
            ServiceCatalog.RegisterService<IHttpClientFactory>(new HttpClientFactory(config));
    }
}
