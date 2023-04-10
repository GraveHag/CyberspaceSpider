using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace CS_Core
{
    /// <summary>
    /// CyberspaceSpider
    /// type of web crawler
    /// </summary>
    internal sealed class CyberspaceSpider : WebCrawler
    {
        public CyberspaceSpider(Func<CrawlerConfiguration> configuration) : base(configuration)
        {
        }

        public CyberspaceSpider() { }

        public override async Task<CrawlerResponse?> Crawl(Uri uri, CancellationToken token)
        {
            if (!IsAlive) return default;

            HttpResponseMessage response = await GetResponse(uri, token);

            LogService.Info($"{GetType().Name}:[{_spiderName}]", nameof(Crawl), $"{uri} [{response.StatusCode}]");

            if (!response.IsSuccessStatusCode) return new CrawlerResponse { CurrentDomain = uri.ToString(), StatusCode = (int)response.StatusCode };

            string htmlContent = await response.Content.ReadAsStringAsync(token);

            CrawlerResponse crawlerResponse = new CrawlerResponse { CurrentDomain = uri.ToString() };

            //Read content -> transfer to document object
            IDocument document = await ReadHtmlContent(htmlContent);

            //Retrieve all "<a href=....><a/>" dom elements, then select href contents to list
            IHtmlCollection<IElement> linkElements = document.Links;

            if (linkElements.Length == 0) return crawlerResponse;

            IList<Uri> links = linkElements.Select(el => ((IHtmlAnchorElement)el).Href).ToUriList();


            //Filter new distinct domains
            crawlerResponse.NextDomains = links.Select(link => link.Host).Distinct().ToUriList();

            return crawlerResponse;

        }
    }
}
