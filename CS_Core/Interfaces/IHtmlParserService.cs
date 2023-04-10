namespace CS_Core
{
    public interface IHtmlParserService
    {
        /// <summary>
        /// Extract distinct domains reachable from provided page
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        Task<IList<Uri>> ExtractDomainsFromContentAsync(string htmlContent);

        /// <summary>
        /// Extract all links from href elements present on page
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        Task<IList<Uri>> ExtractLinksFromContentAsync(string htmlContent);

        /// <summary>
        /// Extract all available meta tags from page content
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        Task<IDictionary<string, string>> ExtractMetaTagsFromContentAsync(string htmlContent);
    }
}
