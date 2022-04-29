using System;
using System.Collections.Concurrent;
using System.Linq;
using TinyUrl.Models;

namespace TinyUrl.Services
{
    public class UrlCache : IUrlCache
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
                        var lessUsedUrl = localCache.Aggregate((l, r) => l.Value.UsageCount < r.Value.UsageCount ? l : r).Key;

                        localCache.TryRemove(lessUsedUrl, out _); 
                    }

                    localCache.TryAdd(url.FullUrl, url);
                }
            }          
        }

        public Url GetValueBykey(string key)
        {
            return localCache.Values.FirstOrDefault(x => x.Key == key); 
        }
    }
}
