using System;

namespace TumblrV2.Helpers
{
    public static class EnumExtenders
    {
        public static bool IsEnumValue(this Enum value) =>
            Enum.IsDefined(value.GetType(), value);
    }
}
