using System.Net;

namespace CS_Core
{
    /// <summary>
    /// Crawler response
    /// </summary>
    public sealed class CrawlerResponse
    {
        public Uri? CurrentDomain { get; set; }

        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

        public IEnumerable<MetaTagModel>? MetaTags { get; set; }

        public IEnumerable<Uri>? NextDomains { get; set; }
    }
}
