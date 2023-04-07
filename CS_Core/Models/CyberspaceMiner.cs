namespace CS_Core
{
    /// <summary>
    /// CyberspaceMiner
    /// type of web crawler that deeply mining on specific domain
    /// </summary>
    internal sealed class CyberspaceMiner : WebCrawler
    {
        public override Task<CrawlerResponse?> GetUrl(Uri uri, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public override Task Run(CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
