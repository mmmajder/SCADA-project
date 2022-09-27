using ReportManager.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportManager
{
    class Program
    {

        public static string MainMenu()
        {
            List<string> validInput = new List<string> { "1", "2", "3", "4", "5", "6" };
            while (true)
            {
                Console.WriteLine("Report manager");
                Console.WriteLine("1) All alarms that happened in the same time period");
                Console.WriteLine("2) All alarms with specific priority");
                Console.WriteLine("3) All values of tags that were measured in specific time preriod");
                Console.WriteLine("4) Last value of every AI tag");
                Console.WriteLine("5) Last value of every DI tag");
                Console.WriteLine("6) All tag values by id");
                string input = Console.ReadLine();
                if (validInput.Contains(input))
                    return input;
                Console.WriteLine("Invalid input, try again!");
            }
            
        }

        public static ServiceReference1.ReportManagerServiceClient reportServiceClient = new ServiceReference1.ReportManagerServiceClient();
        static void Main(string[] args)
        {
            while (true)
            {
                string selection = MainMenu();
                switch (selection)
                {
                    case "1":
                        AlarmsInSameTimePeriod();
                        break;
                    case "2":
                        AlarmsWithPriority();
                        break;
                    case "3":
                        TagsInSameTimePeriod();
                        break;
                    case "4":
                        LastValueOfEveryAI();
                        break;
                    case "5":
                        LastValueOfEveryDI();
                        break;
                    case "6":
                        TagValuesById();
                        break;
                }
                Console.WriteLine("Press any key for next selection...");
                Console.ReadKey();
            }
        }

        private static void TagValuesById()
        {
            Console.WriteLine("Enter tag name:");
            string input = Console.ReadLine();
            TagInvoked[] tagInvokeds = reportServiceClient.AllValuesOfTag(input);
            if (tagInvokeds.Count()==0)
            {
                Console.WriteLine("There is no invoked tag with that name");
            }
            foreach (TagInvoked tagInvoked in tagInvokeds)
            {
                PrintTag(tagInvoked);
            }
        }

        private static void LastValueOfEveryAI()
        {
            foreach (TagInvoked tagInvoked in reportServiceClient.LastValueOfAITags())
            {
                PrintTag(tagInvoked);
            }
        }

        private static void LastValueOfEveryDI()
        {
            foreach (TagInvoked tagInvoked in reportServiceClient.LastValueOfDITags())
            {
                PrintTag(tagInvoked);
            }
        }



        private static void TagsInSameTimePeriod()
        {
            Console.Write("Enter start date (MM/DD/YYYY HH:MM:SS): ");
            DateTime start = InputService.EnterDateTime();
            Console.Write("Enter end date (MM/DD/YYYY HH:MM:SS): ");
            DateTime end = InputService.EnterDateTime();

            TagInvoked[] tags = reportServiceClient.TagInvokecInSameTimePeriod(start, end);
            foreach (TagInvoked tagInvoked in tags)
            {
                PrintTag(tagInvoked);
            }
        }

        private static void PrintTag(TagInvoked tagInvoked)
        {
            Console.WriteLine("Tag name: " + tagInvoked.TagName + " | Value: " + tagInvoked.Value + " | Time: " + tagInvoked.TimeStamp);
        }

        private static void PrintAlarm(AlarmInvoked alarmInvoked)
        {
            Console.WriteLine("Alarm for tag: " + alarmInvoked.TagName + " | Type: " + alarmInvoked.Type + " | Priority: " + alarmInvoked.Priority + " | Limit: " + alarmInvoked.Limit + " | Time: " + alarmInvoked.TimeStamp);
        }

        private static void AlarmsWithPriority()
        {
            Console.Write("Enter priority: ");
            int priority = InputService.EnterPriority();
            AlarmInvoked[] alarms = reportServiceClient.AlarmsByPriority(priority);
            foreach (AlarmInvoked alarmInvoked in alarms)
            {
                PrintAlarm(alarmInvoked);
            }

        }

        private static void AlarmsInSameTimePeriod()
        {
            Console.Write("Enter start date (MM/DD/YYYY HH:MM:SS): ");
            DateTime start = InputService.EnterDateTime();
            Console.Write("Enter end date (MM/DD/YYYY HH:MM:SS): ");
            DateTime end = InputService.EnterDateTime();

            AlarmInvoked[] alarms = reportServiceClient.AlarmsInSameTimePeriod(start, end);
            foreach (AlarmInvoked alarmInvoked in alarms)
            {
                PrintAlarm(alarmInvoked);
            }
        }

    }
}
