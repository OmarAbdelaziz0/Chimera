using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using MauiApp6.model;
namespace MauiApp6
{
    public class MongoDBService
    {
        private readonly IMongoCollection<PersonModel> _persons;

        public MongoDBService()
        {
            var client = new MongoClient("mongodb+srv://user1:eclair%40123@chimera.l1lfq.mongodb.net/?retryWrites=true&w=majority&appName=Chimera");
            var database = client.GetDatabase("Chimera");
            _persons = database.GetCollection<PersonModel>("Chimera");
        }

        // This method checks if the user exists by either email or username and verifies the password
        public async Task<bool> ValidateUserAsync(string usernameOrEmail, string password, bool isEmailLogin, bool isUsernameLogin)
        {
            var filter = Builders<PersonModel>.Filter.Eq(isEmailLogin ? "Email" : "Username", usernameOrEmail);
            var person = await _persons.Find(filter).FirstOrDefaultAsync();

            if (person == null) return false; // User not found

            // Compare passwords (in a real application, you should hash passwords and compare the hashes)
            return person.Password == password;
        }

        // Insert person method
        public async Task InsertPersonAsync(PersonModel person)
        {
            await _persons.InsertOneAsync(person);
        }
    }
}
