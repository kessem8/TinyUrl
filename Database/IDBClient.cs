using MongoDB.Driver;

namespace TinyUrl.Database
{
    public interface IDBClient
    {
        IMongoDatabase MongoDatabase { get; set; }     
    }
}
