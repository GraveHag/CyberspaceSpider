using System.Net;

namespace CS_Core
{
    /// <summary>
    /// ProxyService
    /// Obtains all free-for-all proxies
    /// </summary>
    internal sealed class ProxyService : IProxyService, IService
    {
        readonly HttpClient Client = new HttpClient();

        ProxyList? _proxyList;

        public ProxyService()
        { 
        }

        async Task<ProxyList> ProxyList() => _proxyList is null ? await GetProxyList() : _proxyList;

        async Task<ProxyList> GetProxyList()
        {
            Uri proxyList = new Uri("https://proxylist.geonode.com/api/proxy-list?limit=20&page=1&sort_by=lastChecked&sort_type=desc&protocols=http");

            using HttpResponseMessage response = await Client.GetAsync(proxyList, CancellationToken.None);

            if (!response.IsSuccessStatusCode) throw new Exception("Cannot create proxy");

            string responseJson = await response.Content.ReadAsStringAsync() ?? throw new Exception("Missing payload");

            _proxyList = System.Text.Json.JsonSerializer.Deserialize<ProxyList>(responseJson) ?? new ProxyList();

            return _proxyList;
        }

        async Task<WebProxy> IProxyService.GetProxy()
        {
            Random rnd = new Random();

            ProxyList list = await ProxyList();

            Proxy proxy = list.Data[rnd.Next(0, list.Data.Count - 1)] ?? throw new Exception("Proxy is null");

            LogService.Info(nameof(ProxyService), nameof(IProxyService.GetProxy), proxy.ToString());

            return new WebProxy($"{Uri.UriSchemeHttp}://{proxy.Ip}", int.Parse(proxy.Port));
        }
    }
}
