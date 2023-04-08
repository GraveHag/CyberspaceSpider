using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Core.Extensions
{
    internal static class IEnumerableExtensions
    {
        internal static IList<Uri> ToUriList(this IEnumerable<string> values)
        {
            IList<Uri> result = new List<Uri>();
            foreach (string value in values)
            {
                if (string.IsNullOrEmpty(value)) continue;
                if (!value.Contains("http"))
                {
                    Uri uri = new Uri("https://" + value);
                    result.Add(uri);
                }
                else 
                {
                    result.Add(new Uri( value));
                }
            }
            return result;
        }
    }
}
