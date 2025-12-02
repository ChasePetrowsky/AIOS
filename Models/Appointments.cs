using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace AIOS.Models
{
    public class Appointment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [BsonElement("customerId")]
        [JsonPropertyName("customerId")]
        public string CustomerId { get; set; } = string.Empty;

        [BsonElement("customerName")]
        [JsonPropertyName("customerName")]
        public string CustomerName { get; set; } = string.Empty;

        [BsonElement("serviceType")]
        [JsonPropertyName("serviceType")]
        public string ServiceType { get; set; } = string.Empty;

        [BsonElement("description")]
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("start")]
        [JsonPropertyName("start")]
        public DateTime Start { get; set; }

        [BsonElement("end")]
        [JsonPropertyName("end")]
        public DateTime End { get; set; }

        [BsonElement("backgroundColor")]
        [JsonPropertyName("backgroundColor")]
        public string BackgroundColor { get; set; } = "#4da3ff";

        [BsonElement("createdAt")]
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}