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
            var isContainingFullUrl = localCache.Any(x => x.Value.FullUrl.Equals(url.FullUrl));
            int currentItemsCount = localCache.Count;
            _readersWritersSync.LeaveRead();

            if (!isContainingFullUrl)
            {

                if (currentItemsCount < Constants.CAPACITY)
                {
                    _readersWritersSync.EnterWrite();
                    localCache.TryAdd(url.Key, url);
                    _readersWritersSync.LeaveWrite();
                }
                else
                {    // check what returns
                    _readersWritersSync.EnterRead();
                    var lessUsedUrl = localCache.Aggregate((l, r) => l.Value.UsageCount < r.Value.UsageCount ? l : r).Key;
                    _readersWritersSync.LeaveRead();

                    _readersWritersSync.EnterWrite();
                    localCache.TryRemove(lessUsedUrl, out _);
                    localCache.TryAdd(url.Key, url);
                    _readersWritersSync.LeaveWrite();
                }
            }
            //else 
            //{
            //    _readersWritersSync.EnterWrite();
            //    localCache[url.Key].UsageCount++;
            //    _readersWritersSync.LeaveWrite();
            //}
        }

        public Url GetValueBykey(string key)
        {
            return localCache[key]; 
        }
    }
}
