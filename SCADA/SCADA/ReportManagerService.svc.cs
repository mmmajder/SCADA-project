using SCADA.Data;
using SCADA.Model;
using SCADA.Model.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SCADA
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ReportManagerService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ReportManagerService.svc or ReportManagerService.svc.cs at the Solution Explorer and start debugging.
    public class ReportManagerService : IReportManagerService
    {
         public List<AlarmInvoked> AlarmsByPriority(int priority)
         {
             using (var db = new AlarmInvokedContext())
             {
                 var alarms = from invokedAlarm in db.AlarmInvokes  
                              where invokedAlarm.Priority == priority
                              orderby invokedAlarm.TimeStamp descending
                              select invokedAlarm;
                 return alarms.ToList();
             }
         }

        public List<AlarmInvoked> AlarmsInSameTimePeriod(DateTime start, DateTime end)
        {
            using (var db = new AlarmInvokedContext())
            {
                var alarms = from invokedAlarm in db.AlarmInvokes
                                where (invokedAlarm.TimeStamp >= start && invokedAlarm.TimeStamp <= end)
                                orderby invokedAlarm.TimeStamp descending
                                orderby invokedAlarm.Priority descending
                                select invokedAlarm;
                return alarms.ToList();
            }
        }

        public List<TagInvoked> AllValuesOfTag(string tagName)
        {
            using (var db = new TagInvokedContext())
            {
                var allInvokedTags = from invokedTag in db.InvokedTags
                                     where invokedTag.TagName == tagName
                                     orderby invokedTag.Value descending
                                     select invokedTag;
                return allInvokedTags.ToList();
            }
        }

        public static bool IsDigitalInput(string tagName)
        {
            Tag tag = TagManager.GetTagById(tagName);
            return tag.TagType == TagType.DI;
        } 

        public static bool IsAnalogInput(string tagName)
        {
            Tag tag = TagManager.GetTagById(tagName);
            return tag.TagType == TagType.AI;
        }

        private class TagInvokedDTO
        {
            public int Id { get; set; }
            public string TagName { get; set; }
            public double Value { get; set; }
            public DateTime TimeStamp { get; set; }
            public TagType TagType { get; set; }

        }

        public List<TagInvoked> LastValueOfAITags()
        {
            using (var db = new TagInvokedContext())
            {
                var lastTagInvokes = (from tagInvoked in db.InvokedTags
                                     where tagInvoked.TagType == TagType.AI
                                   //  orderby tagInvoked.TimeStamp descending
                                     group tagInvoked by tagInvoked.TagName into g
                                     
                                     select new TagInvokedDTO
                                     {
                                         Id = g.OrderByDescending(x => x.TimeStamp).FirstOrDefault().Id,
                                         TagName = g.OrderByDescending(x => x.TimeStamp).FirstOrDefault().TagName,
                                         TimeStamp = g.OrderByDescending(x => x.TimeStamp).FirstOrDefault().TimeStamp,
                                         Value = g.OrderByDescending(x => x.TimeStamp).FirstOrDefault().Value,
                                         TagType = g.OrderByDescending(x => x.TimeStamp).FirstOrDefault().TagType
                                     });
                return ConverteFromDTO(lastTagInvokes.ToList());
            }
        }

        private static List<TagInvoked> ConverteFromDTO(List<TagInvokedDTO> tempList)
        {
            List<TagInvoked> tagInvokeds = new List<TagInvoked>();
            foreach (TagInvokedDTO temp in tempList)
            {
                tagInvokeds.Add(new TagInvoked(temp.Id, temp.TagName, temp.Value, temp.TimeStamp, temp.TagType));
            }
            return tagInvokeds;
        }

        public List<TagInvoked> LastValueOfDITags()
        {
            using (var db = new TagInvokedContext())
            {
                
                var lastTagInvokes = (from tagInvoked in db.InvokedTags
                                     where tagInvoked.TagType == TagType.DI
                                   //  orderby tagInvoked.TimeStamp descending
                                     group tagInvoked by tagInvoked.TagName into g
                                     select new TagInvokedDTO
                                     {
                                         Id = g.OrderByDescending(x => x.TimeStamp).FirstOrDefault().Id,
                                         TagName = g.OrderByDescending(x => x.TimeStamp).FirstOrDefault().TagName,
                                         TimeStamp = g.OrderByDescending(x => x.TimeStamp).FirstOrDefault().TimeStamp,
                                         Value = g.OrderByDescending(x => x.TimeStamp).FirstOrDefault().Value,
                                         TagType = g.OrderByDescending(x => x.TimeStamp).FirstOrDefault().TagType
                                     });
                return ConverteFromDTO(lastTagInvokes.ToList());
            }
        }

        public List<TagInvoked> TagInvokecInSameTimePeriod(DateTime start, DateTime end)
        {
            using (var db = new TagInvokedContext())
            {
                var tagsInvoked = from tagInvoked in db.InvokedTags
                                  where (tagInvoked.TimeStamp >= start && tagInvoked.TimeStamp <= end)
                                  orderby tagInvoked.TimeStamp descending
                                  select tagInvoked;
                return tagsInvoked.ToList();
            }
        }

       
    }
}
