using SCADA.Model.Alarms;
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
    public class AlarmInvoked
    {
        //Added all Alarm params needed for reports because didn't work with param Alarm

        [Key]
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public int AlarmId { get; set; }
        [DataMember]
        public AlarmType Type { get; set; }
        [DataMember]
        public int Priority { get; set; }
        [DataMember]
        public double Limit { get; set; }
        [DataMember]
        public string Units { get; set; }
        [DataMember]
        public string TagName { get; set; }
        [DataMember]
        public DateTime TimeStamp { get; set; }
         
    }
}