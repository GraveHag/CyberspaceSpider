namespace CS_Core
{
    /// <summary>
    /// ICrawlerFactoryExtensions
    /// extensions for crawler factory
    /// </summary>
    public static class IWebCrawlerMindExtensions
    {
        public static SpiderMother Configure(this SpiderMother webCrawlerMind, Action<SpiderMotherConfigurationBuilder> builder) {

            return webCrawlerMind;
        }
    }
}
