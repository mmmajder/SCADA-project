using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SCADA.Model.Tags
{
    [DataContract]
    public class OutputTag : Tag
    {
        [DataMember]
        public double InitialValue { get; set; }
        public OutputTag(string tagName, string description, string ioaddress, TagType tagType, bool isDeleted, double initialValue) : base(tagName, description, ioaddress, tagType, isDeleted)
        {
            InitialValue = initialValue;
        }
    }
}