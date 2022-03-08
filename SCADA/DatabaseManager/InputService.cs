using DatabaseManager.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager
{
    class InputService
    {
        public static AlarmType EnterAlarmType()
        {
            while (true)
            {
                Console.WriteLine("Enter alarm type");
                string input = Console.ReadLine();
                if (input.ToLower() == "high")
                    return AlarmType.high;
                if (input.ToLower() == "low")
                    return AlarmType.low;
                Console.WriteLine("Invalid input, try again!");
            }
        }

        public static int EnterPriority()
        {
            while (true)
            {
                Console.WriteLine("Enter alarm priority");
                string input = Console.ReadLine();
                List<string> validInput = new List<string> { "1", "2", "3" };
                if (validInput.Contains(input))
                    return Int32.Parse(input);
                Console.WriteLine("Invalid input, try again!");
            }
        }

        public static double EnterDouble()
        {
            while (true)
            {
                string input = Console.ReadLine();
                try
                {
                    double ret = Double.Parse(input);
                    return ret;
                }
                catch(Exception)
                {
                    Console.WriteLine("Invalid input, try again!");
                }
            }
        }
    }
}
