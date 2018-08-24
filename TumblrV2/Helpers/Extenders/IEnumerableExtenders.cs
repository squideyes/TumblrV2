using System;
using System.Collections.Generic;
using System.Linq;

namespace TumblrV2.Helpers
{
    public static class IListExtenders
    {
        public static bool HasElements<T>(
            this IEnumerable<T> items, Func<T, bool> isValid = null)
        {
            if (items == null)
                return false;

            if (!items.Any())
                return false;

            if (items.Any(i => i.Equals(default(T))))
                return false;

            if (isValid != null && !items.All(i => isValid(i)))
                return false;

            return true;
        }
    }
}
