using System;

namespace TumblrV2.Helpers
{
    public class MinMax<T> where T : IComparable<T>
    {
        public MinMax(T minValue, T maxValue)
        {
            if (maxValue.CompareTo(minValue) < 0)
                throw new ArgumentOutOfRangeException(nameof(maxValue));

            MinValue = minValue;
            MaxValue = maxValue;
        }

        public T MinValue { get; }
        public T MaxValue { get; }
    }
}
