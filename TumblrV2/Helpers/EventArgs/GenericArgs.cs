using System;

namespace TumblrV2.Helpers
{
    public class GenericArgs<T> : EventArgs 
    {
        public GenericArgs(T data)
        {
            Data = data;
        }

        public T Data { get; }
    }
}
