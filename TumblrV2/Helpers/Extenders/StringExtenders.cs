using System;
using System.Collections.Generic;
using System.IO;

namespace TumblrV2.Helpers
{
    public static class StringExtenders
    {
        public static T ToEnum<T>(this string value) =>
            (T)Enum.Parse(typeof(T), value, true);

        public static bool ToEnumList<T>(
            this List<string> values, out List<T> items)
            where T : struct
        {
            items = new List<T>();

            if (values.Count == 0)
                return false;

            var joined = string.Join("", values)
                .Replace(" ", "").Split(',');

            foreach (var value in joined)
            {
                if (!Enum.TryParse(value, true, out T symbol))
                    return false;

                items.Add(symbol);
            }

            return (items.Count > 0);
        }
    }
}