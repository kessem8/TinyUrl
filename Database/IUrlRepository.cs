using TinyUrl.Models;

namespace TinyUrl.Database
{
    public interface IUrlRepository
    {
        Url GetUrlByKey(string key);
        Url GetUrlByFull(string fullUrl);
        bool IsExistByFull(string fullUrl);
        bool IsExistByKey(string key);
        void Add(Url url);
        void UpdateCounter(string fullUrl);
    }
}