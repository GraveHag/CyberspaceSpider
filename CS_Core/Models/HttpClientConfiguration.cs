namespace CS_Core
{
    /// <summary>
    /// HttpClientConfiguration
    /// </summary>
    public sealed class HttpClientConfiguration
    {
        /// <summary> HttpTimeout</summary>
        public TimeSpan HttpTimeout { get; set; }

        /// <summary> allow auto redirect </summary>
        public bool AllowAutoRedirect { get; set; } = true;

        /// <summary> use proxy </summary>
        public bool UseProxy { get; set; } = false;
    }
}
