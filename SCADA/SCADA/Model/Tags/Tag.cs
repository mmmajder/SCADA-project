using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SCADA.Model.Tags
{
    [DataContract]
    public class Tag
    {
        [DataMember]
        public string TagName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string IOaddress { get; set; }
        [DataMember]
        public TagType TagType { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        public Tag(string tagName, string description, string oaddress, TagType tagType, bool isDeleted)
        {
            TagName = tagName;
            Description = description;
            IOaddress = oaddress;
            TagType = tagType;
            IsDeleted = isDeleted;
        }
    }
}