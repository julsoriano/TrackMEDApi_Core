using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TrackMEDApi
{
    public interface IEntity
    {
        [BsonId]
        string Id { get; set; }
    }
}
