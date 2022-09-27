using AlarmDisplay.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AlarmDisplay
{
    class Callback : IAlarmDisplayServiceCallback
    {
        private void SetColor(int priotity)
        {
            switch (priotity)
            {
                case 1:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case 3:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }
        }

        public void FireAlarm(AlarmInvoked alarmInvoked)
        {
            SetColor(alarmInvoked.Priority);
            for (int i=0; i<alarmInvoked.Priority; i++)
            {
                PrintAlarm(alarmInvoked);
            }
        }

        public void PrintAlarm(AlarmInvoked alarmInvoked)
        {
            string output = "Alarm: " + alarmInvoked.Id;
            output += "| Type: " + alarmInvoked.Type;
            output += "| Limit: " + alarmInvoked.Limit;
            output += "| Unit: " + alarmInvoked.Units;
            output += "| Time stamp: " + alarmInvoked.TimeStamp;
            output += "| For tag: " + alarmInvoked.TagName;

            Console.WriteLine(output);
        }
    }


    class Program
    {
        static InstanceContext ic = new InstanceContext(new Callback());
        static ServiceReference1.AlarmDisplayServiceClient alarmClient = new AlarmDisplayServiceClient(ic);
        static void Main(string[] args)
        {
            alarmClient.Init();
            while(true) { }
        }
    }
}
