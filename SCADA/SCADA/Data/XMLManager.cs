using SCADA.Model.Alarms;
using SCADA.Model.Tags;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace SCADA.Data
{
    public class XMLManager
    {
        public static void LoadTags()
        {
            XElement xml = XElement.Load("C:/Users/Korisnik/Desktop/SNUSProjekat/SCADA/SCADA/Data/scadaConfig.xml");

            foreach (XElement xmlTag in xml.Descendants("Tag"))
            {
                string tagName = xmlTag.Attribute("TagName").Value;
                string description = xmlTag.Attribute("Description").Value;
                string ioaddress = xmlTag.Attribute("IOAddress").Value;
                bool isDeleted = xmlTag.Attribute("IsDeleted").Value == "true" ? true : false;
                switch (xmlTag.Attribute("Type").Value)
                {
                    case "AI":
                        int scanTime = Int32.Parse(xmlTag.Attribute("ScanTime").Value);
                        bool isActiveScan = xmlTag.Attribute("IsActiveScan").Value == "on" ? true : false;
                        DriverType driverType = xmlTag.Attribute("Driver").Value == "Simulation" ? DriverType.Simulation : DriverType.RealTime;
                        double low = Double.Parse(xmlTag.Attribute("LowLimit").Value);
                        double high = Double.Parse(xmlTag.Attribute("HighLimit").Value);
                        string units = xmlTag.Attribute("Units").Value;
                        AI ai = new AI(tagName, description, ioaddress, TagType.AI, isDeleted, scanTime, isActiveScan, driverType, low, high, units);
                        TagManager.InputTags.Add(ai);
                        TagManager.AnalogInputTags.Add(ai);
                        break;
                    case "AO":
                        double initialValue = Double.Parse(xmlTag.Attribute("InitialValue").Value);
                        low = Double.Parse(xmlTag.Attribute("LowLimit").Value);
                        high = Double.Parse(xmlTag.Attribute("HighLimit").Value);
                        units = xmlTag.Attribute("Units").Value;
                        AO ao = new AO(tagName, description, ioaddress, TagType.AO, isDeleted, initialValue, low, high, units);
                        TagManager.OutputTags.Add(ao);
                        TagManager.AnalogOutputTags.Add(ao);
                        break;
                    case "DI":
                        scanTime = Int32.Parse(xmlTag.Attribute("ScanTime").Value);
                        isActiveScan = xmlTag.Attribute("IsActiveScan").Value == "on" ? true : false;
                        driverType = xmlTag.Attribute("Driver").Value == "Simulation" ? DriverType.Simulation : DriverType.RealTime;
                        DI di = new DI(tagName, description, ioaddress, TagType.DI, isDeleted, scanTime, isActiveScan, driverType);
                        TagManager.InputTags.Add(di);
                        TagManager.DigitalInputTags.Add(di);
                        break;
                    case "DO":
                        initialValue = Double.Parse(xmlTag.Attribute("InitialValue").Value);
                        DO digitalOutput = new DO(tagName, description, ioaddress, TagType.DO, isDeleted, initialValue);
                        TagManager.OutputTags.Add(digitalOutput);
                        TagManager.DigitalOutputTags.Add(digitalOutput);
                        break;
                    default:
                        break;
                }
            }
        }

        public static void WriteTags()
        {
            XElement tags = new XElement("Tags");
            foreach (AI ai in TagManager.AnalogInputTags)
            {
                string isActiveScan = ai.IsActiveScan == true ? "on" : "off";

                XElement tag = new XElement("Tag");
                tag.SetAttributeValue("TagName", ai.TagName);
                tag.SetAttributeValue("Description", ai.Description);
                tag.SetAttributeValue("IOAddress", ai.IOaddress);
                tag.SetAttributeValue("Type", "AI");
                tag.SetAttributeValue("IsDeleted", ai.IsDeleted);
                tag.SetAttributeValue("ScanTime", ai.ScanTime);
                tag.SetAttributeValue("IsActiveScan", isActiveScan);
                tag.SetAttributeValue("Driver", ai.Driver);
                tag.SetAttributeValue("LowLimit", ai.LowLimit);
                tag.SetAttributeValue("HighLimit", ai.HighLimit);
                tag.SetAttributeValue("Units", ai.Units);
                tags.Add(tag);
            }

            foreach (AO ao in TagManager.AnalogOutputTags)
            {
                XElement tag = new XElement("Tag");
                tag.SetAttributeValue("TagName", ao.TagName);
                tag.SetAttributeValue("Description", ao.Description);
                tag.SetAttributeValue("IOAddress", ao.IOaddress);
                tag.SetAttributeValue("Type", "AO");
                tag.SetAttributeValue("IsDeleted", ao.IsDeleted);
                tag.SetAttributeValue("InitialValue", ao.InitialValue);
                tag.SetAttributeValue("LowLimit", ao.LowLimit); 
                tag.SetAttributeValue("HighLimit", ao.HighLimit);
                tag.SetAttributeValue("Units", ao.Units);
                tags.Add(tag);
            }

            foreach (DI di in TagManager.DigitalInputTags)
            {
                string isActiveScan = di.IsActiveScan == true ? "on" : "off";

                XElement tag = new XElement("Tag");
                tag.SetAttributeValue("TagName", di.TagName);
                tag.SetAttributeValue("Description", di.Description);
                tag.SetAttributeValue("IOAddress", di.IOaddress);
                tag.SetAttributeValue("Type", "DI");
                tag.SetAttributeValue("IsDeleted", di.IsDeleted);
                tag.SetAttributeValue("ScanTime", di.ScanTime);
                tag.SetAttributeValue("IsActiveScan", isActiveScan);
                tag.SetAttributeValue("Driver", di.Driver);
                tags.Add(tag);
            }

            foreach (DO digitalOutput in TagManager.DigitalOutputTags)
            {
                XElement tag = new XElement("Tag");
                tag.SetAttributeValue("TagName", digitalOutput.TagName);
                tag.SetAttributeValue("Description", digitalOutput.Description);
                tag.SetAttributeValue("IOAddress", digitalOutput.IOaddress);
                tag.SetAttributeValue("Type", "DO");
                tag.SetAttributeValue("IsDeleted", digitalOutput.IsDeleted);
                tag.SetAttributeValue("InitialValue", digitalOutput.InitialValue);
                tags.Add(tag);
            }

            tags.Save("C:/Users/Korisnik/Desktop/SNUSProjekat/SCADA/SCADA/Data/scadaConfig.xml");

        }
        //

        public static void ReadAlarmsXML()
        {
            XElement xml = XElement.Load("C:/Users/Korisnik/Desktop/SNUSProjekat/SCADA/SCADA/Data/alarmConfig.xml");

            foreach (XElement xmlAlarm in xml.Descendants("Alarm"))
            {
                string tagName = xmlAlarm.Attribute("TagName").Value;
                AlarmType alarmType = xmlAlarm.Attribute("AlarmType").Value == "low" ? AlarmType.low : AlarmType.high;

                int Priority = Int32.Parse(xmlAlarm.Attribute("Priority").Value);
                int Limit = Int32.Parse(xmlAlarm.Attribute("Limit").Value);
                int id = Int32.Parse(xmlAlarm.Attribute("Id").Value);
                Alarm alarm = new Alarm(id, TagManager.GetAITag(tagName), alarmType, Priority, Limit);

                if (TagProcessing.alarms.ContainsKey(tagName))
                {
                    TagProcessing.alarms[tagName].Add(alarm);
                }
                else
                {
                    TagProcessing.alarms[tagName] = new List<Alarm> { alarm };
                }
            }
        }

        public static void WriteAlarmsXML()
        {
            XElement tags = new XElement("Alarms");
            foreach (string tagName in TagProcessing.alarms.Keys)
            {
                foreach (Alarm alarm in TagProcessing.alarms[tagName])
                {
                    XElement tag = new XElement("Alarm");
                    tag.SetAttributeValue("Id", alarm.Id);
                    tag.SetAttributeValue("TagName", alarm.Tag.TagName);
                    tag.SetAttributeValue("AlarmType", alarm.Type);
                    tag.SetAttributeValue("Priority", alarm.Priority);
                    tag.SetAttributeValue("Limit", alarm.Limit);
                    tags.Add(tag);
                }
            }
            tags.Save("C:/Users/Korisnik/Desktop/SNUSProjekat/SCADA/SCADA/Data/alarmConfig.xml");
        }

    }
}