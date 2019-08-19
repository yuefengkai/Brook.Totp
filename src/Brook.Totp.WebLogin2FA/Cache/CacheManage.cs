using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brook.Totp.WebLogin2FA.Cache
{
    public class CacheManage : ICacheManage
    {
        private IMemoryCache _cache;
        public CacheManage(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public T Get<T>(string key)
        {
            if (!_cache.TryGetValue(key, out object cacheEntry))
            {
                return default;
            }

            return (T)cacheEntry;

        }

        public void Set(string key, object value, int seconds = int.MaxValue)
        {
            _cache.Set(key, value, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(seconds)));
        }


        public T GetOrCreate<T>(string key, Func<ICacheEntry, T> factory)
        {
            var value = _cache.GetOrCreate(key, factory);

            return value;
        }
        

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

    }
}
