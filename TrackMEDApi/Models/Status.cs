using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace TrackMEDApi.Models
{
    public class Status: IEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [Required(ErrorMessage = "A status description is required")]
        [DisplayName("Status")]
        [JsonProperty(PropertyName = "desc")]
        public string Desc { get; set; }

        [JsonProperty(PropertyName = "createdAtUtc")]
        public DateTime CreatedAtUtc { get; set; }
    }
}