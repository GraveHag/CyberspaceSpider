using System.Diagnostics.CodeAnalysis;

namespace CS_Core
{
    /// <summary>
    /// MetaTagComparator
    /// </summary>
    internal class MetaTagComparator : IEqualityComparer<MetaTagModel>
    {
        public bool Equals(MetaTagModel? x, MetaTagModel? y) {

            if (x is null || y is null) return false;
            if (x.Name is null || y.Name is null ) return false;

            return x.Name.Equals(y.Name);
        }

        public int GetHashCode([DisallowNull] MetaTagModel obj) => obj.GetHashCode() ^ obj.GetHashCode();
    }
}
