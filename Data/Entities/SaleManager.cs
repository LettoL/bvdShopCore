using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Data.Entities
{
    public class SaleManager
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string ManagerId { get; set; }

        public int SaleId { get; set; }
    }
}