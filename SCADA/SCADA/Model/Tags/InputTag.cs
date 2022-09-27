using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SCADA.Model.Tags
{
    [DataContract]
    public class InputTag : Tag
    {
        [DataMember]
        public int ScanTime { get; set; }
        [DataMember]
        public bool IsActiveScan { get; set; }
        [DataMember]
        public DriverType Driver { get; set; }

        public InputTag(string tagName, string description, string ioaddress, TagType tagType, bool isDeleted, int scanTime, bool isActiveScan, DriverType driver) : base(tagName, description, ioaddress, tagType, isDeleted)
        {
            ScanTime = scanTime;
            IsActiveScan = isActiveScan;
            Driver = driver;
        }
    }
}