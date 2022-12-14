using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Project_NetCore_MongoDB.Models
{
    public class Cars
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        [Required]
        public string? Name { get; set; }

        [BsonElement("type_car")]
        public string? Type_Car { get; set; }

        [BsonElement("consumption")]
        public int? Consumption { get; set; }

        [BsonElement("license_plates")]
        public string? License_Plates { get; set; }

        [BsonElement("gasoline_fuel")]
        public string? Gasoline_Fuel { get; set; }

        [BsonElement("oil_fuel")]
        public string? Oil_Fuel { get; set; }
    }
}
