﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TrackMEDApi.Models
{
    public class Deployment: IEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string DeploymentID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yy}")]
        //[DisplayFormatAttribute(ApplyFormatInEditMode = true, DataFormatString = "{0:ddMMMyy hh:mm}")]
        [Display(Name = "Deployment Date")]
        public DateTime DeploymentDate { get; set; }

        public string SystemTabID { get; set; }
        public string LocationID { get; set; }

        [Display(Name = "Reference No.")]
        public string ReferenceNo { get; set; }

        //[Timestamp]
        //public byte[] RowVersion { get; set; }

        public virtual SystemTab SystemTab { get; set; }
        public virtual Location Location { get; set; }
    }
}