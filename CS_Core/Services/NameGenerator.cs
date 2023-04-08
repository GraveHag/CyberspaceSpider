namespace CS_Core
{
    /// <summary>
    /// Name generator for web crawlers
    /// </summary>
    internal static class NameGenerator
    {
        static string[][] spiderNames = new string[][] {
            new string[] {"ArachnoByte", "WebWeaver", "CobwebCrawler", "InsectaBot", "ByteBite", "BugHunter", "CyberneticArachnid", "SpidrByte", "NetWeaver", "RoboSpider"},
            new string[] {"SilkWorm", "Spidrman", "CyberSpider", "WebCrawler", "SteelWeaver", "ByteSpider", "VenomByte", "ArachniDroid", "SpiderByte", "WebSurgeon"},
            new string[] {"CodeCrawler", "ArachniBot", "WebWarrior", "CyberCobweb", "NetSurgeon", "ByteCrawler", "BlackWidow", "InsectaByte", "WebWarper", "CodeWeaver"}
        };

        public static string GetName()
        {
            Random rand = new Random();
            int outerIndex = rand.Next(0, spiderNames.Length);
            int innerIndex = rand.Next(0, spiderNames[outerIndex].Length);

            return spiderNames[outerIndex][innerIndex];
        }
    }
}
