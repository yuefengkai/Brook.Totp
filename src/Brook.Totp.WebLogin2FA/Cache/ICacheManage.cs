using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brook.Totp.WebLogin2FA.Cache
{
    public interface ICacheManage
    {
        T Get<T>(string key);

        void Set(string key, object value, int seconds = int.MaxValue);

        TItem GetOrCreate<TItem>(string key, Func<ICacheEntry, TItem> factory);

        void Remove(string key);
    }
}
