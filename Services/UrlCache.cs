using System;
using System.Collections.Concurrent;
using System.Linq;
using TinyUrl.Models;

namespace TinyUrl.Services
{
    public class UrlCache : IUrlCache
    {
        private readonly ConcurrentDictionary<string, Url> localCache = new ConcurrentDictionary<string, Url>();
        private static ReaderWritersSync _readersWritersSync = new ReaderWritersSync();
        public void Add(Url url)
        {
            _readersWritersSync.EnterRead();
            bool isContainingKey = localCache.ContainsKey(url.FullUrl);
            int currentItemsCount = localCache.Count;
            _readersWritersSync.LeaveRead();

            if (!isContainingKey)
            {

                if (currentItemsCount <= Constants.CAPACITY)
                {
                    _readersWritersSync.EnterWrite();
                    localCache.TryAdd(url.FullUrl, url);
                    _readersWritersSync.LeaveWrite();
                }
                else
                {
                    _readersWritersSync.EnterRead();
                    var lessUsedUrl = localCache.Aggregate((l, r) => l.Value.UsageCount < r.Value.UsageCount ? l : r).Key;
                    _readersWritersSync.LeaveRead();

                    _readersWritersSync.EnterWrite();
                    localCache.TryRemove(lessUsedUrl, out _);
                    localCache.TryAdd(url.FullUrl, url);
                    _readersWritersSync.LeaveWrite();
                }
            }
            else 
            {
                _readersWritersSync.EnterWrite();
                localCache[url.FullUrl].UsageCount++;
                _readersWritersSync.LeaveWrite();
            }


        }


        public Url GetValueBykey(string key)
        {
            // return returnUrl;
            return localCache.Values.FirstOrDefault(x => x.Key == key); 
        }
    }
}
