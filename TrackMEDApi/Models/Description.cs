
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TrackMEDApi.Models
{
    public class Description: IEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "A description of the equipment is required")]
        [DisplayName("Description")]
        public string Desc { get; set; }

        public string Tag { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}
