using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportManager
{
    public class InputService
    {
        public static DateTime EnterDateTime()
        {
            DateTime dateValue;
            while (true)
            {
                string dateString = Console.ReadLine();
                if (DateTime.TryParse(dateString, out dateValue))
                    return dateValue;
                Console.WriteLine("Invalid input!");
            }
        }

        public static int EnterPriority()
        {
            List<string> validInput = new List<string> { "1", "2", "3" };
            while (true)
            {
                string input = Console.ReadLine();
                if (validInput.Contains(input))
                {
                    return Int32.Parse(input);
                }
                Console.WriteLine("Enter number 1, 2 or 3");
            }
        }
    }
}
