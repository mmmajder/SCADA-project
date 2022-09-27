using SCADA.Model.Tags;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SCADA.Model.Alarms
{
    [DataContract]
    public class Alarm
    {
        [Key]
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public AI Tag { get; set; }
        [DataMember]
        public AlarmType Type { get; set; }
        [DataMember]
        public int Priority { get; set; }
        [DataMember]
        public double Limit { get; set; }

        public Alarm() { }

        public Alarm(int id, AI tag, AlarmType type, int priority, double limit)
        {
            Id = id;
            Tag = tag;
            Type = type;
            Priority = priority;
            Limit = limit;
        }

    }
}