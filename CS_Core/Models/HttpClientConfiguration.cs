namespace CS_Core
{
    /// <summary>
    /// HttpClientConfiguration
    /// </summary>
    public sealed class HttpClientConfiguration
    {
        public TimeSpan Timeout { get; set; }

        public bool AllowAutoRedirect = true;

        public bool UseProxy = false;
    }
}
