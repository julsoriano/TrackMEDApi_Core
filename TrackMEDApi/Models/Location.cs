using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TrackMEDApi.Models
{
    public class Location: IEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "A location name is required")]
        [DisplayName("Location")]
        public string Desc { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}