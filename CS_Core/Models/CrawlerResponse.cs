
namespace CS_Core
{
    /// <summary>
    /// Crawler response
    /// </summary>
    public sealed class CrawlerResponse
    {
        public string? CurrentDomain { get; set; }

        public int StatusCode { get; set; } = 200;

        public IEnumerable<MetaTagModel>? MetaTags { get; set; }

        public IEnumerable<Uri>? NextDomains { get; set; }
    }
}
