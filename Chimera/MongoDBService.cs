using MongoDB.Driver;

namespace Chimera.Services
{
    public class MongoDBService
    {
        private readonly IMongoDatabase _database;

        public MongoDBService()
        {
            var connectionString = "mongodb+srv://user1:eclair%40123@chimera.l1lfq.mongodb.net/?retryWrites=true&w=majority&appName=Chimera";
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("Chimera"); // Ensure database name is correct
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}
