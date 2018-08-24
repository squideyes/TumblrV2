using System;
using System.Linq;
using System.Reflection;

namespace TumblrV2.Helpers
{
    public class EnumList<T> : ListBase<T> where T : struct
    {
        public EnumList()
        {
            if (!typeof(T).GetTypeInfo().IsEnum)
            {
                throw new ArgumentException(
                    "The generic type must be an Enum.");
            }

            Items = Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }
    }
}
