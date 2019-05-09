using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TrackMEDApi.Models
{
    public class Owner: IEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "Owner name is required")]
        [DisplayName("Owner")]
        public string Desc { get; set; }

        public DateTime CreatedAtUtc { get; set; }

    }
}