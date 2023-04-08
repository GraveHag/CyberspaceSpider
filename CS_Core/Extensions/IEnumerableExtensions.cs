namespace CS_Core
{
    /// <summary>
    /// IEnumerableExtensions
    /// </summary>
    internal static class IEnumerableExtensions
    {
        public static IList<Uri> ToUriList(this IEnumerable<string> values)
        {
            IList<Uri> result = new List<Uri>();

            Parallel.ForEach(values.Where(s => !string.IsNullOrEmpty(s)), value =>
            {
                result.Add(new Uri(!value.Contains(Uri.UriSchemeHttp) ? $"{Uri.UriSchemeHttps}://{value}" : value));
            });

            return result;

        }
    }
}
