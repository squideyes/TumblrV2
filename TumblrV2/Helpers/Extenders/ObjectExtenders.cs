namespace TumblrV2.Helpers
{
    public static class ObjectExtenders
    {
        public static bool IsDefault<T>(this T value) => Equals(value, default(T));
    }
}
