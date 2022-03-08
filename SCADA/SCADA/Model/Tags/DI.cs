using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SCADA.Model.Tags
{
    [DataContract]
    public class DI : InputTag
    {
        public DI(string tagName, string description, string ioaddress, TagType tagType, bool isDeleted, int scanTime, bool isActiveScan, DriverType driver) : base(tagName, description, ioaddress, tagType, isDeleted, scanTime, isActiveScan, driver)
        {
        }
    }
}