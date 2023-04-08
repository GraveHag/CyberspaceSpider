using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using CS_Core.Extensions;

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

        public override async Task<CrawlerResponse?> GetUrl(Uri uri, CancellationToken token)
        {
            if (!IsAlive) return default;

            HttpResponseMessage? response = await GetResponse(uri, token);
            if (response is null) return new CrawlerResponse { CurrentDomain = uri.ToString(), StatusCode = 500};
            if (!response.IsSuccessStatusCode) return new CrawlerResponse { CurrentDomain = uri.ToString(), StatusCode = (int)response.StatusCode };

            string htmlContent = await response.Content.ReadAsStringAsync(token);

            CrawlerResponse crawlerResponse = new CrawlerResponse { CurrentDomain = uri.ToString() };

            //Read content -> transfer to document object
            IDocument document = await ReadHtmlContent(htmlContent);

            //Retrieve all "<a href=....><a/>" dom elements, then select href contents to list
            IHtmlCollection<IElement> linkElements = document.QuerySelectorAll("a");
            IList<Uri> links = linkElements.Select(el => ((IHtmlAnchorElement)el).Href).ToUriList();

            // -OPTIONAL-
            // Check collection for valid HTTP/HTTPS, remove the rest. Reversed iteration enables immediate removal of items while iterating.
            //
            //foreach(Uri link in links.Reverse()) 
            //{
            //    bool result = (link.Scheme == Uri.UriSchemeHttp || link.Scheme == Uri.UriSchemeHttps);
            //    if (!result)
            //    {
            //        links.Remove(link);
            //    }
            //}

            //Filter new distinct domains
            crawlerResponse.NextDomains = links.Select(link => link.Host).Distinct().ToUriList();

            return crawlerResponse;

        }

        public override async Task Run(CancellationToken token)
        {
            //todo another possibilities 
            while (IsAlive)
            {
                await GetUrl(StartedDomain, token);
            }

        }
    }
}
