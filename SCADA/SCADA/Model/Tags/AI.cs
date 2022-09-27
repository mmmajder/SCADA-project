using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SCADA.Model.Tags
{
    [DataContract]
    public class AI : InputTag
    {
        [DataMember]
        public double LowLimit { get; set; }
        [DataMember]
        public double HighLimit { get; set; }
        [DataMember]
        public string Units { get; set; }
        public AI(string tagName, string description, string ioaddress, TagType tagType, bool isDeleted, int scanTime, bool isActiveScan, DriverType driver, double low, double high, string units) : base(tagName, description, ioaddress, tagType, isDeleted, scanTime, isActiveScan, driver)
        {
            LowLimit = low;
            HighLimit = high;
            Units = units;
        }
    }
}