using MongoDB.Driver;

namespace TinyUrl.Database
{
    public class DBClient : IDBClient
    {
        private const string DB = "TinyUrlDB";
        public IMongoDatabase MongoDatabase { get; set; }

        public DBClient(string connectionString)
        {
            MongoClient client = new MongoClient(connectionString);
            MongoDatabase = client.GetDatabase(DB);
        }        
    }
}
