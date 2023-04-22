using CS_Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CyberspaceSpider.Pages
{
    public class IndexModel : PageModel
    {

        [BindProperty]
        public CrawlerConfiguration crawlerConfiguration { get; set; } = new CrawlerConfiguration();

        [BindProperty]
        public HttpClientConfiguration httpClientConfiguration { get; set; } = new HttpClientConfiguration();

        public void OnGet()
        {
            crawlerConfiguration = ServiceCatalog.Mediate<IConfigurationService>().CrawlerConfiguration ?? new CrawlerConfiguration();
            httpClientConfiguration = ServiceCatalog.Mediate<IConfigurationService>().HttpClientConfiguration ?? new HttpClientConfiguration();
        }

        public async Task<IActionResult> OnPost(CancellationToken token)
        {
            Configuration config = new Configuration
            {
                HttpClientConfiguration = httpClientConfiguration,
                CrawlerConfiguration = crawlerConfiguration
            };
            ServiceCatalog.ReplaceService<IConfigurationService>(new ConfigurationService(config));

            SpiderMother mother = new SpiderMother(crawlerConfiguration);
            await mother.Run(token);

            return new JsonResult(new { Success = true/*, Data = mother.Results*/ });
        }
    }
}