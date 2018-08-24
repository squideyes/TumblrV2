using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TumblrV2.Helpers
{
    public abstract class ListBase<T> : IEnumerable<T>
    {
        protected List<T> Items = new List<T>();

        public int Count => Items.Count;

        public T First => Items.First();

        public T FirstOrDefault => Items.FirstOrDefault();

        public T Last => Items.Last();

        public T LastOrDefault => Items.LastOrDefault();

        public bool HasElements(Func<T, bool> isValid = null) =>
            Items.HasElements(isValid);

        public void ForEach(Action<T> action) => 
            Items.ForEach(i => action(i));

        public IEnumerator<T> GetEnumerator() => 
            Items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
