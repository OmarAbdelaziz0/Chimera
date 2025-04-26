using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MauiApp6.Models
{
    public class PersonModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }  // Changed from ObjectId to string

        [BsonElement("Username")]
        public string Username { get; set; }

        [BsonElement("FirstName")]
        public string FirstName { get; set; }

        [BsonElement("LastName")]
        public string LastName { get; set; }

        [BsonElement("Age")]
        public int Age { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonElement("Gender")]
        public string Gender { get; set; }

        [BsonElement("Password")]
        public string Password { get; set; }

        [BsonElement("Latitude")]
        public double Latitude { get; set; }

        [BsonElement("Longitude")]
        public double Longitude { get; set; }

        [BsonElement("Distance")]
        public double Distance { get; set; }

        [BsonElement("Role")]
        public string Role { get; set; }

        [BsonElement("PhoneNumber")]
        public string PhoneNumber { get; set; }

        // Constructor to ensure required fields are initialized
        public PersonModel()
        {
            Id = ObjectId.GenerateNewId().ToString(); // Ensure ID is set when creating a new person
        }
    }
}
