using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TumblrV2.Helpers
{
    public static class MiscExtenders
    {
        public static R Funcify<T, R>(this T value, Func<T, R> func) => func(value);

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> items) =>
            (items == null) || (!items.Any());

        public static T GetAttribute<T>(this Assembly callingAssembly)
            where T : Attribute
        {
            T result = null;

            var configAttributes = Attribute.
                GetCustomAttributes(callingAssembly, typeof(T), false);

            if (!configAttributes.IsNullOrEmpty())
                result = (T)configAttributes[0];

            return result;
        }

        public static bool IsNonEmptyAndTrimmed(this string value)
        {
            return !string.IsNullOrWhiteSpace(value)
                && !char.IsWhiteSpace(value[0])
                && !char.IsWhiteSpace(value[value.Length - 1]);
        }

        public static bool IsMatch(this string value,
            string pattern, RegexOptions options = RegexOptions.None)
        {
            return new Regex(pattern, options).IsMatch(value);
        }

        public static void EnsurePathExists(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentOutOfRangeException(nameof(value));

            value = Path.GetFullPath(value);

            var folder = Path.GetDirectoryName(value);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
        }
        public static bool IsFolderName(
            this string value, bool mustBeRooted = true)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            try
            {
                new DirectoryInfo(value);

                if (!mustBeRooted)
                    return true;
                else
                    return Path.IsPathRooted(value);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (PathTooLongException)
            {
                return false;
            }
            catch (NotSupportedException)
            {
                return false;
            }
        }
    }
}
