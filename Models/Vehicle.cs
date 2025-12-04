using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AIOS.Models
{
    public class Vehicle
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Make { get; set; } = "";
        public string Model { get; set; } = "";
        public int Year { get; set; }
        public string VIN { get; set; } = "";
        public int Kms { get; set; }

        public string Status { get; set; } = "In Stock";

        // URL to photo stored in /wwwroot/uploads or cloud storage
        public string PhotoUrl { get; set; } = "";
    }
}
