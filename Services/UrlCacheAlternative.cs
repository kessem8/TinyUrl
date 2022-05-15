using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyUrl.Models;

namespace TinyUrl.Services
{
    public class UrlCacheAlternative : IUrlCache
    {
        private readonly static object locker = new object();
        private readonly ConcurrentDictionary<string, Url> localCache = new ConcurrentDictionary<string, Url>();

        public void Add(Url url)
        {
            if (!localCache.ContainsKey(url.FullUrl))
            {
                if (localCache.Count <= Constants.CAPACITY)
                {
                    localCache.TryAdd(url.FullUrl, url);
                }
                else
                {
                    lock (locker)
                    {
                        var lastCreatedUrl = localCache.Aggregate((l, r) => l.Value.CreationTime < r.Value.CreationTime ? l : r).Key;

                        localCache.TryRemove(lastCreatedUrl, out _);
                    }

                    localCache.TryAdd(url.FullUrl, url);
                }
            }
            return;
        }

        public Url GeUrlBykey(string key)
        {
            return localCache.Values.FirstOrDefault(x => x.Key == key); 
        }

    }
}
