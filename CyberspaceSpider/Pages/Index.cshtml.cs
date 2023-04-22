using CS_Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CyberspaceSpider.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public CrawlerConfiguration crawlerConfiguration { get; set; }

        public void OnGet()
        {
            crawlerConfiguration = ServiceCatalog.Mediate<IConfigurationService>().CrawlerConfiguration ?? new CrawlerConfiguration();
        }

        public async Task<IActionResult> OnPost(CancellationToken token)
        {
            ;
            SpiderMother mother = new SpiderMother(crawlerConfiguration);
            await mother.Run(token);

            return new JsonResult(new { Success = true, Data = mother.Results });
        }
    }
}