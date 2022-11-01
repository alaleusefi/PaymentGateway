using System;
using System.Collections.Generic;

namespace Core
{
    public interface IRepository<T>
    {
        int Save(T item);
        T Get(int id);
    }

    public class FakeRepo<T> : IRepository<T>
    {
        private readonly Dictionary<int, T> items = new Dictionary<int, T>();
        public int Save(T item)
        {
            var nextId = items.Count;
            items.Add(nextId, item);
            return nextId;
        }
        public T Get(int id)
        {
            if (items.ContainsKey(id))
                return items[id];
            return default;
        }
    }
}