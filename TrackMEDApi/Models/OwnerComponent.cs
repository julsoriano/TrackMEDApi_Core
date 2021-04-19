using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TrackMEDApi.Models
{
    public class OwnerComponent : IEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "Owner name is required")]
        [DisplayName("OwnerComponent")]
        public string Desc { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public int NoOfComponents { get; set; }
    }
}