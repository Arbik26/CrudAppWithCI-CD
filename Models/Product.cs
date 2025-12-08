using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CrudApp.Models
{
    public class Product
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }
    }
}
