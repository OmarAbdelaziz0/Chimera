using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MauiApp6.Models;

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

        // Validate User Login with Hashed Password
        public async Task<bool> ValidateUserAsync(string usernameOrEmail, string password, bool isEmailLogin, bool isUsernameLogin)
        {
            var filter = Builders<PersonModel>.Filter.Eq(isEmailLogin ? "Email" : "Username", usernameOrEmail);
            var person = await _persons.Find(filter).FirstOrDefaultAsync();

            if (person == null)
            {
                Console.WriteLine("User not found.");
                return false; // User not found
            }

            return VerifyPassword(password, person.Password);
        }

        // Insert a new person with password hashing
        public async Task InsertPersonAsync(PersonModel person)
        {
            person.Password = HashPassword(person.Password);
            await _persons.InsertOneAsync(person);
            Console.WriteLine($"User '{person.Username}' inserted successfully.");
        }

        // Update role of a person
        public async Task<bool> UpdatePersonRoleAsync(string username, string newRole)
        {
            var filter = Builders<PersonModel>.Filter.Eq("Username", username);
            var update = Builders<PersonModel>.Update.Set("Role", newRole);

            var result = await _persons.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        // Update phone number of a person
        public async Task<bool> UpdatePersonPhoneNumberAsync(string username, string newPhoneNumber)
        {
            var filter = Builders<PersonModel>.Filter.Eq("Username", username);
            var update = Builders<PersonModel>.Update.Set("PhoneNumber", newPhoneNumber);

            var result = await _persons.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        // Get users by role (Rider or Driver)
        public async Task<List<PersonModel>> GetUsersByRoleAsync(string role)
        {
            var filter = Builders<PersonModel>.Filter.Eq("Role", role);
            return await _persons.Find(filter).ToListAsync();
        }

        // Get users by gender (or all users if no preference)
        public async Task<List<PersonModel>> GetUsersByGenderAsync(string gender)
        {
            if (gender == "Any") // Return all users if gender is not specified
                return await _persons.Find(_ => true).ToListAsync();

            var filter = Builders<PersonModel>.Filter.Eq("Gender", gender);
            return await _persons.Find(filter).ToListAsync();
        }

        // Get users by role and gender
        public async Task<List<PersonModel>> GetUsersByRoleAndGenderAsync(string role, string gender)
        {
            var filter = Builders<PersonModel>.Filter.Eq("Role", role);
            if (gender != "Any")
            {
                var genderFilter = Builders<PersonModel>.Filter.Eq("Gender", gender);
                filter = Builders<PersonModel>.Filter.And(filter, genderFilter);
            }
            return await _persons.Find(filter).ToListAsync();
        }

        // Get nearest user based on location, gender, and role
        public async Task<PersonModel?> GetNearestUserAsync(double userLat, double userLon, string role, string gender)
        {
            var users = await GetUsersByRoleAndGenderAsync(role, gender);
            if (users == null || !users.Any()) return null;

            return users
                .Select(user => new { User = user, Distance = CalculateDistance(userLat, userLon, user.Latitude, user.Longitude) })
                .OrderBy(u => u.Distance)
                .FirstOrDefault()?.User;
        }

        // Get user by username
        public async Task<PersonModel> GetUserByUsernameAsync(string username)
        {
            var filter = Builders<PersonModel>.Filter.Eq("Username", username);
            return await _persons.Find(filter).FirstOrDefaultAsync();
        }

        // Hash password using SHA256
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        // Verify hashed password
        private bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            return HashPassword(enteredPassword) == storedHashedPassword;
        }

        // Calculate distance between two latitude/longitude points
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double R = 6371; // Radius of Earth in km
            double dLat = (lat2 - lat1) * (Math.PI / 180);
            double dLon = (lon2 - lon1) * (Math.PI / 180);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1 * (Math.PI / 180)) * Math.Cos(lat2 * (Math.PI / 180)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
    }
}
