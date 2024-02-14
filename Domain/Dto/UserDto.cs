using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Domain.Dto
{
    public class UserDto
    {
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
    }
}
