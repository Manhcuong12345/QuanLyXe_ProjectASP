using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Project_NetCore_MongoDB.Models
{
    public class Drivers
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("carId")]
        public string? CarId { get; set; }

        [BsonElement("car")]
        public Cars Cars { get; set; }

    }
}
