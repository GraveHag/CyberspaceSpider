﻿namespace CS_Core
{
    /// <summary>
    /// CyberspaceMiner
    /// type of web crawler that deeply mining on specific domain
    /// </summary>
    internal sealed class CyberspaceMiner : WebCrawler
    {
        public async override Task<CrawlerResponse> Crawl(Uri uri, CancellationToken token)
        {
            HttpResponseMessage response = await GetResponse(() => new HttpRequestMessage(HttpMethod.Get,uri), token);

            throw new NotImplementedException();
        }

    }
}
