using MongoDB.Bson.Serialization.Attributes;

namespace Project_NetCore_MongoDB.Dto
{
    public class AuthsDto
    {

        [BsonElement("UserName")]
        public string? UserName { get; set; }

        [BsonElement("Password")]
        public string? Password { get; set; }
    }
}
