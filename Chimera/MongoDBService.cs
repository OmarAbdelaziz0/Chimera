using MongoDB.Driver;
using MauiApp6;  
public class MongoDBService
{
    private readonly IMongoCollection<PersonModel> _collection;

    public MongoDBService()
    {
        string connectionString = "mongodb+srv://user1:eclair%40123@chimera.l1lfq.mongodb.net/?retryWrites=true&w=majority&appName=Chimera";
        var client = new MongoClient(connectionString);
        var db = client.GetDatabase("Chimera");
        _collection = db.GetCollection<PersonModel>("Chimera");
    }

    internal async Task InsertPersonAsync(PersonModel person)
    {
        await _collection.InsertOneAsync(person);
    }
}
