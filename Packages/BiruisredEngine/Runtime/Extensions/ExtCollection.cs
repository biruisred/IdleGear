using System.Collections.Generic;

namespace BiruisredEngine
{
    public static class ExtCollection
    {
        public static bool NotContains<T>(this List<T> list, T item)
        {
            return !list.Contains(item);
        }
    }
}