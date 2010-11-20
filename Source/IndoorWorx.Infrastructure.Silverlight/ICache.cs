using System;

namespace IndoorWorx.Infrastructure
{
    public interface ICache
    {
        void Add(string key, object data, TimeSpan validDuration);
        
        object Get(string key);

        T Get<T>(string key) where T : class;
        
        bool Remove(string key);
    }
}
