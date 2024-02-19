using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Domain.Dto
{
    public class UserRegisterDto
    {
        [BsonElement("username")]
        public string UserName { get; set; } = null!;

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("lastname")]
        public string? LastName { get; set; }
        [BsonElement("age")]
        public string? Age { get; set; }

        [BsonElement("email")]
        public string? Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; } = null!;
    }
}
