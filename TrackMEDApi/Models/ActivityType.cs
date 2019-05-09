using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TrackMEDApi.Models
{
    public class ActivityType: IEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "Activity Type is required")]
        [DisplayName("Activity")]
        public string Desc { get; set; }

        public DateTime CreatedAtUtc { get; set; }

    }
}