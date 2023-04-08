using System.Text.Json.Serialization;

namespace CS_Core
{
    /// <summary>
    /// Proxy list
    /// contains list of proxy
    /// </summary>
    public sealed class ProxyList
    {
        [JsonPropertyName("data")]
        public List<Proxy> Data { get; set; } = new List<Proxy>();
    }

    /// <summary>
    /// proxy
    /// represent ip and port for proxy
    /// </summary>
    public sealed class Proxy
    {
        [JsonPropertyName("ip")]
        public string Ip { get; set; } = string.Empty;

        [JsonPropertyName("port")]
        public string Port { get; set; } = string.Empty;


        public override string ToString() => $"{Ip}:{Port}";
       
    }

    
}
