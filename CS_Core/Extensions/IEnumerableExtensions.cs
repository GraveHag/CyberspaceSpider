using System.Collections.Concurrent;

namespace CS_Core
{
    /// <summary>
    /// IEnumerableExtensions
    /// </summary>
    internal static class IEnumerableExtensions
    {
        public static IList<Uri> ToUriList(this IEnumerable<string> values)
        {
            ConcurrentBag<Uri> result = new ConcurrentBag<Uri>();

            Parallel.ForEach(values.Where(s => !string.IsNullOrEmpty(s)), value =>
            {
                string scheme = value.Contains(':') ? value.Split(':')[0] : string.Empty;
                if (Uri.CheckSchemeName(scheme))
                {
                    if (value.Contains(Uri.UriSchemeHttp) || value.Contains(Uri.UriSchemeHttps))
                    {
                        result.Add(new Uri(value));
                    }
                }
                else
                {
                    result.Add(new Uri($"{Uri.UriSchemeHttps}://{value}"));
                }
            });

            return result.ToList();

        }
    }
}
