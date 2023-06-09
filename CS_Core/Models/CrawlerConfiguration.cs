﻿namespace CS_Core
{
    /// <summary>
    /// Crawler configuration
    /// </summary>
    public sealed class CrawlerConfiguration
    {
        /// <summary> Crawler live</summary>
        public TimeSpan TimeToLive { get; set; } = new TimeSpan(0, 0, 10);

        /// <summary> Crawler time to rest before another request</summary>
        public TimeSpan TimeToRest { get; set; } = new TimeSpan(0, 0, 1);

        /// <summary> Crawler type</summary>
        public CrawlerType CrawlerType { get; set; }

        /// <summary>max depth crawling on current domain</summary>
        public int MaxDepth { get; set; }

        /// <summary> Numbers of spiders that can be use in runs </summary>
        public int MaxSpiders { get; set; } = 1;

        /// <summary> Started list of domains </summary>
        public string StartedDomain { get; set; } = string.Empty;

        /// <summary> blacklist domains</summary>
        public string[]? Blacklist { get; set; }

        /// <summary> MaxDomainsToVisit</summary>
        public int MaxDomainsToVisit { get; set; } = 25;

    }
}
