using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;
using TinyUrl.Models;

namespace TinyUrl.Database
{

    public class UrlRepository : IUrlRepository
    {
        private readonly IMongoCollection<Url> _urls;


        public UrlRepository(IDBClient dBClient)
        {
            _urls = dBClient.MongoDatabase.GetCollection<Url>(typeof(Url).Name);
        }

        public Url GetFullByKey(string key)
        {
            return _urls.Find(x => x.Key == key).First();
        }

        public Url GetUrlByFull(string fullUrl)
        {
            return _urls.Find(x => x.FullUrl == fullUrl).First();
        }

        public bool IsExistByFull(string fullUrl)
        {
            return _urls.Find(x => x.FullUrl == fullUrl).Any();
        }

        public bool IsExistByKey(string key)
        {
            return _urls.Find(x => x.Key == key).Any();
        }

        public void Add(Url newUrl)
        {
            _urls.InsertOne(newUrl);
        }

        public void UpdateCounter(string fullUrl)
        {
            var filter = Builders<Url>.Filter.And(Builders<Url>.Filter.Eq("FullUrl", fullUrl));
            var updates = Builders<Url>.Update.Inc("UsageCount", 1);
            _urls.UpdateOne(filter, updates);
        }


    }
}
