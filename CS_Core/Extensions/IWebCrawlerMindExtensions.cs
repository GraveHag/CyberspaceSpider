namespace CS_Core
{
    /// <summary>
    /// ICrawlerFactoryExtensions
    /// extensions for crawler factory
    /// </summary>
    public static class IWebCrawlerMindExtensions
    {
        public static WebCrawlerMind UseBlackList(this WebCrawlerMind webCrawler, string fileName) {

            string jsonResult = ServiceCatalog.Mediate<IFileService>().LoadFileContent(fileName);

            IList<Uri> blackList = new List<Uri>();

            //todo

            return webCrawler;
        }
    }
}
