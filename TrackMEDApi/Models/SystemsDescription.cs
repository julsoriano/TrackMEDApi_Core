using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TrackMEDApi.Models
{
    public class SystemsDescription: IEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "A description of the system is required")]
        [DisplayName("Systems Description")]
        public string Desc { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}
