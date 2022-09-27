using SCADA.Model;
using SCADA.Model.Alarms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SCADA.Data
{
    public class AlarmManager
    {
        private static void WriteInvokeAlarmTxt(AlarmInvoked alarmInvoked)
        {
            Alarm Alarm = GetAlarmById(alarmInvoked.AlarmId);
            using (StreamWriter sw = File.AppendText("C:/Users/Korisnik/Desktop/SNUSProjekat/SCADA/SCADA/Data/alarmsLog.txt"))
            {
                sw.WriteLine("Alarm type: " + Alarm.Type + " | Priority: " + Alarm.Priority + " | Limit: " + Alarm.Limit + " | Unit: " + Alarm.Tag.Units + " | TimeStamp: " + alarmInvoked.TimeStamp);
            }
        }

        public static Alarm GetAlarmById(int id)
        {
            foreach (string tagName in TagProcessing.alarms.Keys)
                foreach (Alarm alarm in TagProcessing.alarms[tagName]) 
                    if (alarm.Id == id)
                        return alarm;
            return null;
        }

        public static void AddInvokedAlarm(AlarmInvoked alarmInvoked)
        {
            WriteInvokeAlarmTxt(alarmInvoked);
            using (var db = new AlarmInvokedContext()) {
                db.AlarmInvokes.Add(alarmInvoked);
                db.SaveChanges();
            }
        }

        public static string GenerateId()
        {
            using (var db = new AlarmInvokedContext())
            {
                return db.AlarmInvokes.Count().ToString();
            }
        }
    }
}