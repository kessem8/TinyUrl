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
        private readonly static object dictionaryLocker = new object();


        public UrlServices(IUrlRepository repository, IUrlCache urlCache)
        {
            this.repository = repository;
            _urlCache = urlCache;
        }

        public Url CreateShortUrl(string fullUrl)
        {
            Url newUrl;

            lock (dictionaryLocker)
            {
                if (!repository.IsExistByFull(fullUrl))
                {
                    newUrl = new Url
                    {
                        Key = KeyGenerator.Instance.CreateMD5Hash(fullUrl),
                        CreationTime = DateTime.Now,
                        FullUrl = fullUrl,
                        Id = Guid.NewGuid().ToString(),
                        UsageCount = 1
                    };

                    repository.Add(newUrl);
                    _urlCache.Add(newUrl);
                }
                else
                {
                    newUrl = repository.GetUrlByFull(fullUrl);
                    int counter = newUrl.UsageCount;
                    newUrl.UsageCount = Interlocked.Increment(ref counter);
                    _urlCache.Add(newUrl);
                }
            }

            return newUrl;
        }

        public string GetFullUrl(string key)
        {            
            //string key = shortUrl.Replace(Constants.HOME_URL, "");
            Url url = _urlCache.GetValueBykey(key);
            if (url != null)
            {
                return url.FullUrl;
            }
            else
            {
                return repository.GetFullByKey(key);
            }            
        }

    }
}
