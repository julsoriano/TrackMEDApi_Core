using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TrackMEDApi.Models
{
    public class SystemTab: Equipment
    {
        public string SystemsDescriptionID { get; set; }
        public string LocationID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yy}")]
        [Display(Name = "Deployment Date")]
        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)]  // http://stackoverflow.com/questions/8063323/how-to-save-date-properly
        public DateTime? DeploymentDate { get; set; }

        [Display(Name = "Reference No.")]
        public string ReferenceNo { get; set; }

        public virtual SystemsDescription SystemsDescription { get; set; }
        public virtual Location Location { get; set; }

        public List<string> leftComponents { get; set; }
        public List<string> rightComponents { get; set; }

    }
}