using System;
using System.ComponentModel.DataAnnotations;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace TrackMEDApi.Models
{
    public abstract class Equipment: IEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string imte { get; set; }
        public string serialnumber { get; set; }
        [StringLength(50)]
        public string Notes { get; set; }
        public DateTime CreatedAtUtc { get; set; }       

        public string OwnerID { get; set; }
        public string StatusID { get; set; }
        public string ActivityTypeID { get; set; }
        public virtual Owner Owner { get; set; }
        public virtual Status Status { get; set; }
        public virtual ActivityType ActivityType { get; set; }
        //public virtual ICollection<Event> Events { get; set; }
    }
}