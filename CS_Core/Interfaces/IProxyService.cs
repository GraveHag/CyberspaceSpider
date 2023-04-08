using System.Net;

namespace CS_Core
{
    /// <summary>
    /// IProxyService
    /// </summary>
    public interface IProxyService
    {
        Task<WebProxy> GetProxy();
    }
}
