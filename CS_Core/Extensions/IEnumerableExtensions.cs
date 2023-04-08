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
                result.Add(new Uri(value, UriKind.RelativeOrAbsolute));
            }
            return result;
        }
    }
}
