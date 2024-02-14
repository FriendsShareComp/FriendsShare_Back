using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; set; }
        [BsonElement("username")]
        public string UserName { get; set; } = null!;

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("lastname")]
        public string LastName { get; set; } = null!;
        [BsonElement("age")]
        public string Age { get; set; } = null!;

        [BsonElement("email")]
        public string Email { get; set; } = null!;

        [BsonElement("password")]
        public string Password { get; set; } = null!;

        [BsonElement("CreatedAt")]
        public string CreatedAt { get; set; } = null!;

        [BsonElement("description")]
        public string Description { get; set; } = null!;

        [BsonElement("UpdateAt")]
        public string UpdateAt { get; set; } = null!;

        [BsonElement("Active")]
        public string Active { get; set; } = null!;

        [BsonElement("FriendsCuantity")]
        public int FriendsCuantity { get; set; }
    }
}
