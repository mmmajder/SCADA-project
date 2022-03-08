using SCADA.Model.Tags;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SCADA.Model
{
    [DataContract]
    public class TagInvoked
    {
        [Key]
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string TagName { get; set; }
        [DataMember]
        public double Value { get; set; }
        [DataMember]
        public DateTime TimeStamp { get; set; }
        [DataMember]
        public TagType TagType { get; set; }

        public TagInvoked(int id, string tagName, double value, DateTime timeStamp, TagType tagType)
        {
            Id = id;
            TagName = tagName;
            Value = value;
            TimeStamp = timeStamp;
            TagType = tagType;
        }

        public TagInvoked() { }
    }
}