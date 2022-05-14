using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using TinyUrl.Database;
using TinyUrl.Models;
using TinyUrl.Utils;

namespace TinyUrl.Services
{
    public class UrlServices : IUrlServices
    {
        private readonly IUrlRepository repository;
        private readonly IUrlCache _urlCache;
        private object _syncToken = new object();


        public UrlServices(IUrlRepository repository, IUrlCache urlCache)
        {
            this.repository = repository;
            _urlCache = urlCache;
        }

        public Url CreateShortUrl(string fullUrl)
        {
            Url newUrl;

            lock (_syncToken)
            {
                if (!repository.IsExistByFull(fullUrl))
                {
                    newUrl = new Url
                    {
                        Key = KeyGenerator.Instance.CreateMD5Hash(fullUrl),
                        CreationTime = DateTime.Now,
                        FullUrl = fullUrl,
                        Id = Guid.NewGuid().ToString(),
                        UsageCount = 0
                    };

                    repository.Add(newUrl);
                }
                else
                {
                    newUrl = repository.GetUrlByFull(fullUrl);
                }
            }
            _urlCache.Add(newUrl);

            return newUrl;
        }

        public string GetFullUrl(string key)
        {
            Url url = _urlCache.GetValueBykey(key) ?? repository.GetFullByKey(key);

            if (url != null)
            {
                int counter = url.UsageCount;
                url.UsageCount = Interlocked.Increment(ref counter);
                repository.UpdateCounter(url.FullUrl);
                return url.FullUrl;
            }
            else
            {
                return null;
            }
        }

    }
}
