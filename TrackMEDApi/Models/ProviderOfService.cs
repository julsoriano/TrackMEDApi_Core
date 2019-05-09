using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TrackMEDApi.Models
{
    public class ProviderOfService: IEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "ServiceProvider name is required")]
        [DisplayName("Service Provider")]
        public string Desc { get; set; }

        public DateTime CreatedAtUtc { get; set; }

    }
}