using SCADA.Data;
using SCADA.Model;
using SCADA.Model.Alarms;
using SCADA.Model.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SCADA
{
    public class TagProcessing
    {
        public static Dictionary<string, Thread> threads = new Dictionary<string, Thread>();
        public static Dictionary<string, List<Alarm>> alarms = new Dictionary<string, List<Alarm>>();
        public static Dictionary<string, List<TagInvoked>> invokedTags = new Dictionary<string, List<TagInvoked>>();

        static readonly object alarmsLocker = new object();
        static readonly object tagInvokeLocker = new object();
        static readonly object threadLocker = new object();


        public static void Init()
        {
            
            TagManager.ReadTags();
            ReadInvokedTags();
            XMLManager.ReadAlarmsXML();
            threads = InitThreads();

        }
        private static void ReadInvokedTags()
        {
            using (var db = new TagInvokedContext())
            {                
                foreach (TagInvoked tagInvoked in db.InvokedTags)
                {
                    lock (tagInvokeLocker)
                        if (invokedTags.ContainsKey(tagInvoked.TagName))
                        {
                            invokedTags[tagInvoked.TagName].Add(tagInvoked);
                        }
                        else
                        {
                            List<TagInvoked> list = new List<TagInvoked>() { tagInvoked };
                            invokedTags.Add(tagInvoked.TagName, list);
                        }
                }
            }
        }


        public static Dictionary<string, Thread> InitThreads()
        {
            Dictionary<string, Thread> threads = new Dictionary<string, Thread>();
            foreach (InputTag inputTag in TagManager.InputTags)
            {
                Thread thread = new Thread(() => GetNewValue(inputTag));
                lock (threadLocker)
                    threads[inputTag.TagName] = thread;
                thread.Start();
            }
            return threads;
        }

        public static void InitThread(InputTag inputTag)
        {
            Thread thread = new Thread(() => GetNewValue(inputTag));
            lock (threadLocker)
                threads[inputTag.TagName] = thread;
            thread.Start();
        }


        public static void GetNewValue(InputTag inputTag)
        {
            while (true)
            {
                inputTag = UpdateTag(inputTag.TagName);
                Thread.Sleep(inputTag.ScanTime * 1000);
                if (inputTag.IsActiveScan && !inputTag.IsDeleted)
                {
                    if (inputTag.Driver == DriverType.Simulation)
                    {
                        SimulationDriverInitNewValue(inputTag);
                    }
                    else
                    {
                        RTUInitNewValue(inputTag);
                    }
                }
            }
        }

        private static InputTag UpdateTag(string TagName)
        {
            return TagManager.GetInputTagById(TagName);
        }

        private static void RTUInitNewValue(InputTag inputTag)
        {
            try {
                double newValue = RTD.ReadValue(inputTag.IOaddress);
                ChangeTagValue(inputTag, newValue);
                FireAlarmIfNeeded(inputTag, newValue);
            }
            catch { }

        }

        private static void SimulationDriverInitNewValue(InputTag inputTag)
        {
            double newValue;
            if (inputTag.TagType == TagType.AI)
            {
                newValue = GetAINewValue(TagManager.GetAITag(inputTag.TagName));
                FireAlarmIfNeeded(inputTag, newValue);
            }
            else
            {
                newValue = GetDINewValue(TagManager.GetDITag(inputTag.TagName));
            }
            ChangeTagValue(inputTag, newValue);
        }

        public static void FireAlarmIfNeeded(InputTag inputTag, double newValue)
        {
            lock (alarmsLocker)
            {
                if (!alarms.ContainsKey(inputTag.TagName))
                    return;
                foreach (Alarm alarm in alarms[inputTag.TagName])
                    if (alarm.Type == AlarmType.high)
                    {
                        if (alarm.Limit < newValue)
                            GenerateInvokedAlarm(alarm);
                    }
                    else
                        if (alarm.Limit > newValue)
                        GenerateInvokedAlarm(alarm);
            }
        }

        public static void GenerateInvokedAlarm(Alarm alarm)
        {
            using (var db = new AlarmInvokedContext())
            {
                string id = AlarmManager.GenerateId();
                AlarmInvoked alarmInvoked = new AlarmInvoked
                {
                    AlarmId = alarm.Id,
                    Id = id,
                    TimeStamp = DateTime.Now,
                    Limit = alarm.Limit,
                    Priority = alarm.Priority,
                    TagName = alarm.Tag.TagName,
                    Type = alarm.Type,
                    Units = alarm.Tag.Units
                };
                AlarmManager.AddInvokedAlarm(alarmInvoked);

                AlarmDisplayService.ActivateAlarmDisplay(alarmInvoked);
            }
        }

        public static void ChangeTagValue(Tag inputTag, double newValue)
        {
            newValue = FitTagValue(inputTag, newValue);
            TagInvoked tagInvoked = new TagInvoked(TagManager.GenerateTagInvokeId(), inputTag.TagName, newValue, DateTime.Now, inputTag.TagType);
            AddTagInvokedToDB(tagInvoked, inputTag.TagName);
            TrendingAppService.UpdateTrendingApp(GetCurrentInvokedTags());
        }

        public static void AddTagInvokedToDB(TagInvoked tagInvoked, string tagName)
        {
            lock (tagInvokeLocker)
            {
                using (var db = new TagInvokedContext())
                {
                    if (!invokedTags.ContainsKey(tagName))
                    {
                        List<TagInvoked> list = new List<TagInvoked>() { tagInvoked };
                        invokedTags.Add(tagInvoked.TagName, list);
                    }
                    else
                    {
                        invokedTags[tagName].Add(tagInvoked);
                    }

                    db.InvokedTags.Add(tagInvoked);
                    db.SaveChanges();
                }
            }
        }

        private static List<TagInvoked> GetCurrentInvokedTags()
        {
            lock (tagInvokeLocker)
            {
                List<TagInvoked> tagInvokeds = new List<TagInvoked>();
                foreach (string tagName in invokedTags.Keys)
                {
                    List<TagType> inputTags = new List<TagType> { TagType.AI, TagType.DI };
                    Tag t = TagManager.GetTagById(tagName);
                    if (inputTags.Contains(t.TagType) && !t.IsDeleted)
                        tagInvokeds.Add(invokedTags[tagName].Last());
                }
                return tagInvokeds;
            }
        }

        private static double GetAINewValue(AI tag)
        {
            double simulatorOutput = SimulationDriver.ReturnValue(tag.IOaddress)/100;
            return Math.Round(tag.LowLimit + simulatorOutput * (tag.HighLimit-tag.LowLimit), 2) ;
        }

        private static double GetDINewValue(DI tag)
        {
            double simulatorOutput = SimulationDriver.ReturnValue(tag.IOaddress) / 100;
            return Math.Round(simulatorOutput, 0);
        }

        public static double FitTagValue(Tag tag, double newValue ) 
        {
            if (tag.TagType==TagType.DI || tag.TagType==TagType.DO)
            {
                if (newValue > 1)
                {
                    newValue = 1;
                } else if (newValue<0) {
                    newValue = 0;
                }
            }
            else
            {
                if (tag.TagType==TagType.AI)
                {
                    AI ai = TagManager.GetAITag(tag.TagName);
                    newValue = GetFixedData(ai.LowLimit, ai.HighLimit, newValue);
                }
                else
                {
                    AO ao = TagManager.GetAOTag(tag.TagName);
                    newValue = GetFixedData(ao.LowLimit, ao.HighLimit, newValue);
                }
            }
            return newValue;

        }

        private static double GetFixedData(double lowLimit, double highLimit, double value)
        {
            if (lowLimit>value)
            {
                value = lowLimit;
            }
            else if (highLimit<value)
            {
                value = highLimit;
            }
            return value;
        }
        public static bool AddAlarm(Alarm alarm)
        {
            lock (alarmsLocker)
            {
                if (TagManager.GetTagById(alarm.Tag.TagName)==null || alarm.Tag.TagType!=TagType.AI)
                    return false;
                if (alarm.Limit< alarm.Tag.LowLimit || alarm.Limit > alarm.Tag.HighLimit)
                    return false;
                if (alarms.ContainsKey(alarm.Tag.TagName))
                        alarms[alarm.Tag.TagName].Add(alarm);
                else
                {
                    List<Alarm> tempList = new List<Alarm> { alarm };
                    alarms[alarm.Tag.TagName] = tempList;
                }
                XMLManager.WriteAlarmsXML();
                return true;
            }
        }

        public static bool RemoveAlarm(string id)
        {
            if (!id.All(char.IsDigit))
                return false;
            lock (alarmsLocker)
            {
                
                foreach (string tagName in alarms.Keys)
                {
                    foreach (Alarm alarm in alarms[tagName])
                    {
                        if (alarm.Id == Int32.Parse(id))
                        {
                            if (alarms[tagName].Count == 1)
                                alarms.Remove(tagName);
                            else
                                alarms[tagName].Remove(alarm);
                            XMLManager.WriteAlarmsXML();
                            return true;
                        }
                    }
                }
                return false;
            }
        }

    }
}