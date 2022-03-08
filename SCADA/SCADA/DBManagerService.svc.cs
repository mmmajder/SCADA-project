using SCADA.Authentification;
using SCADA.Data;
using SCADA.Model;
using SCADA.Model.Alarms;
using SCADA.Model.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SCADA
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DBManagerService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DBManagerService.svc or DBManagerService.svc.cs at the Solution Explorer and start debugging.
    public class DBManagerService : IDBManagerService, IAuthentication
    {
        private static Dictionary<string, User> authenticatedUsers = new Dictionary<string, User>();

        public bool ChangeOutputValue(string tagName, double value)
        {
            OutputTag outputTag = TagManager.GetOutputTagById(tagName);
            if (outputTag!=null)
            {
                if (outputTag.IsDeleted)
                    return false;
                value = TagProcessing.FitTagValue(outputTag, value);
                outputTag.InitialValue = value;
                XMLManager.WriteTags();
                TagInvoked tagInvoked = new TagInvoked(TagManager.GenerateTagInvokeId(), tagName, value, DateTime.Now, outputTag.TagType);
                TagProcessing.AddTagInvokedToDB(tagInvoked, tagName);
                return true;
            }
            return false;
        }

        public string GetOutputValues()
        {
            string ret = "";
            foreach (OutputTag outputTag in TagManager.OutputTags)
            {
                if (!outputTag.IsDeleted)
                    ret += "Tag name: " + outputTag.TagName + " | Value: " + outputTag.InitialValue + "\n";
            }
            return ret;
        }

        public bool ChangeScanStatus(string tagName, bool status)
        {
            InputTag inputTag = TagManager.GetInputTagById(tagName);
            if (inputTag==null)
            {
                return false;
            }
            if (inputTag.IsDeleted)
                return false;
            inputTag.IsActiveScan = status;
            XMLManager.WriteTags();
            return true;
        }

        public string GetInputTags()
        {
            string ret = "";

            foreach (InputTag inputTag in TagManager.GetInputTags())
            {
                string scan = inputTag.IsActiveScan == true ? "on" : "off";
                if (!inputTag.IsDeleted)
                    ret += "Tag name: " + inputTag.TagName + " | Scan: " + scan + "\n";
            }
            return ret;
        }

        public string Login(string username, string password)
        {
            
            using (var db = new UsersContext())
            {
                foreach (var user in db.Users)
                {
                    if (username == user.Username && Auth.ValidateEncryptedData(password, user.Password))
                    {
                        string token = Auth.GenerateToken(username);
                        authenticatedUsers.Add(token, user);
                        //TagManager.ReadTags();
                        return token;
                    }
                }
            }
            return "Login failed";

        }

        public bool Logout(string token)
        {
            return authenticatedUsers.Remove(token);
        }

        public bool Registration(string username, string password)
        {
            string encryptedPassword = Auth.EncryptData(password);
            User user = new User(username, encryptedPassword);
            using (var db = new UsersContext())
            {
                try
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return true;
        }

        

        public bool AddAITag(AI ai)
        {
            if (TagManager.GetTagById(ai.TagName)==null)
            {
                TagManager.AnalogInputTags.Add(ai);
                TagManager.InputTags.Add(ai);
                XMLManager.WriteTags();
                TagProcessing.InitThread(ai);
                return true;
            }
            return false;
            
        }

        public bool AddAOTag(AO ao)
        {
            if (TagManager.GetTagById(ao.TagName) == null)
            {
                TagManager.AnalogOutputTags.Add(ao);
                TagManager.OutputTags.Add(ao);
                XMLManager.WriteTags();
                return true;
            }
            return false;
        }

        public bool AddDITag(DI di)
        {
            if (TagManager.GetTagById(di.TagName) == null)
            {
                TagManager.DigitalInputTags.Add(di);
                TagManager.InputTags.Add(di);
                XMLManager.WriteTags();
                TagProcessing.InitThread(di);
                return true;
            }
            return false;
        }

        public bool AddDOTag(DO digitalOutput)
        {
            if (TagManager.GetTagById(digitalOutput.TagName) == null)
            {
                TagManager.DigitalOutputTags.Add(digitalOutput);
                TagManager.OutputTags.Add(digitalOutput);
                XMLManager.WriteTags();
                return true;
            }
            return false;
        }

        public string GetAllTags()
        {
            string ret = "";
            foreach (Tag tag in TagManager.GetTags())
            {
                if (!tag.IsDeleted)
                    ret += "Tag name: " + tag.TagName + " | Description: " + tag.Description + " | I/O address: " + tag.IOaddress + "\n";

            }
            return ret;
        }

        public bool RemoveTag(string id)
        {
            Tag tag = TagManager.GetTagById(id);
            if (tag != null)
            {
                tag.IsDeleted = true;
                
                XMLManager.WriteTags();
                return true;
            }
            return false;
        }

        public string GetAITags()
        {
            string ret = "";
            foreach (AI tag in TagManager.GetAITags())
            {
                if (!tag.IsDeleted)
                    ret += "Tag name: " + tag.TagName + " | Description: " + tag.Description + " | I/O address: " + tag.IOaddress + " | Low limit: " + tag.LowLimit + " | High limit: " + tag.HighLimit + " | Unit: " + tag.Units + "\n";

            }
            return ret;
        }

        public bool AddAlarm(Alarm alarm)
        {
            return TagProcessing.AddAlarm(alarm);
        }

        public bool RemoveAlarm(string id)
        {
            return TagProcessing.RemoveAlarm(id);
        }

        public AI GetAIById(string id)
        {
            foreach (AI ai in TagManager.AnalogInputTags)
            {
                if (ai.TagName==id)
                {
                    return ai;
                }
            }
            return null;
        }
        public string GetAlarms()
        {
            string ret="";
            foreach (string tagName in TagProcessing.alarms.Keys)
            {
                foreach (Alarm alarm in TagProcessing.alarms[tagName])
                {
                    ret += "Id: " + alarm.Id + " | For tag: " + tagName + " | Type: " + alarm.Type + " | Priority: " + alarm.Priority + " | Limit: " + alarm.Limit + "\n";
                } 

            }
            return ret;
        }
    }
}
