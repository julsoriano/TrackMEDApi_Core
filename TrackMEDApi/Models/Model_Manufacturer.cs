using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TrackMEDApi.Models
{
    public class Model_Manufacturer: IEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "A model/manufacturer description is required")]
        [DisplayName("Model/Manufacturer")]
        public string Desc { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}