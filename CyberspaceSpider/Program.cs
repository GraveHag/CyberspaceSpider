using CS_Core;
using Microsoft.AspNetCore.Authentication;

namespace CyberspaceSpider
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            ServiceCatalog.RegisterAllService();

            CrawlerConfiguration crawlerConfiguration = new CrawlerConfiguration();
            HttpClientConfiguration httpClientConfiguration = new HttpClientConfiguration();

            builder.Configuration.GetSection("CrawlerConfiguration").Bind(crawlerConfiguration);
            builder.Configuration.GetSection("HttpClientConfiguration").Bind(httpClientConfiguration);

            Configuration config = new Configuration
            {
                HttpClientConfiguration = httpClientConfiguration,
                CrawlerConfiguration = crawlerConfiguration
            };

            IConfigurationService configuration = new ConfigurationService(config);

            ServiceCatalog.RegisterService<IConfigurationService>(configuration);

            ServiceCatalog.RegisterService<IHttpClientFactory>(new HttpClientFactory(() =>
            {
                return configuration.HttpClientConfiguration;

            }));

            WebApplication? app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}