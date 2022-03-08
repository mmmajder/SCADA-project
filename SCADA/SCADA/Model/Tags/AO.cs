using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SCADA.Model.Tags
{
    [DataContract]
    public class AO : OutputTag
    {
        [DataMember]
        public double LowLimit { get; set; }
        [DataMember]
        public double HighLimit { get; set; }
        [DataMember]
        public string Units { get; set; }
        public AO(string tagName, string description, string ioaddress, TagType tagType, bool isDeleted, double initialValue, double low, double high, string units) : base(tagName, description, ioaddress, tagType, isDeleted, initialValue)
        {
            LowLimit = low;
            HighLimit = high;
            Units = units;
        }
    }
}