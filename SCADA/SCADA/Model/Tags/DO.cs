using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SCADA.Model.Tags
{
    [DataContract]
    public class DO : OutputTag
    {

        public DO(string tagName, string description, string ioaddress, TagType tagType, bool isDeleted, double initialValue) : base(tagName, description, ioaddress, tagType, isDeleted, initialValue)
        {
        }
    }
}