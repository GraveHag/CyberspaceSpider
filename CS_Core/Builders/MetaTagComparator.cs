namespace CS_Core
{
    internal class MetaTagComparator : IEqualityComparer<MetaTagModel>
    {
        public bool Equals(MetaTagModel x, MetaTagModel y) {
            return x.Name.Equals(y.Name);
        }

        public int GetHashCode(MetaTagModel obj) {
            return obj.GetHashCode() ^ obj.GetHashCode();
        } 
    }
}
