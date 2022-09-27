using DatabaseManager.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DatabaseManager
{
    class Program
    {
        public static string token;
        public static ServiceReference1.AuthenticationClient authProxy = new ServiceReference1.AuthenticationClient();
        public static ServiceReference1.DBManagerServiceClient dbManagerProxy = new ServiceReference1.DBManagerServiceClient();
        static void Main(string[] args)
        {
            LoginMenu();
            MainMenu();
            Console.ReadKey();
        }

        private static void MainMenu()
        {
            Console.WriteLine("1) Get output values");
            Console.WriteLine("2) Change output value");
            Console.WriteLine("3) Turn scan on");
            Console.WriteLine("4) Turn scan off");
            Console.WriteLine("5) Add tags");
            Console.WriteLine("6) Remove tags");
            Console.WriteLine("7) Register user");
            Console.WriteLine("8) Add alarm");
            Console.WriteLine("9) Delete alarm");
            Console.WriteLine("x) Logout user");
            string input = Console.ReadLine();
            RunOperation(input);
        }

        private static void RunOperation(string input)
        {
            switch (input)
            {
                case "1":
                    Console.WriteLine(dbManagerProxy.GetOutputValues());
                    break;
                case "2":
                    ChangeOutputValue();
                    break;
                case "3":
                    TurnScanOn();
                    break;
                case "4":
                    TurnScanOff();
                    break;
                case "5":
                    CreateTag();
                    break;
                case "6":
                    RemoveTags();
                    break;
                case "7":
                    RegisterUser();
                    break;
                case "8":
                    AddAlarm();
                    break;
                case "9":
                    RemoveAlarm();
                    break;
                case "x":
                    Logout();
                    break;
                default:
                    MainMenu();
                    break;
            }
            if (input!="x") { MainMenu(); }
        }

        

        private static void Logout()
        {
            if (authProxy.Logout(token))
            {
                LoginMenu();
                MainMenu();
            }
            else
            {
                Console.WriteLine("Something went wrong");
            }
        }

        private static void AddAlarm()
        {
            Console.WriteLine("AI tags");
            Console.WriteLine(dbManagerProxy.GetAITags());
            Console.WriteLine("Enter name of tag for alarm");
            string tagName = Console.ReadLine();
            AI ai = dbManagerProxy.GetAIById(tagName);
            AlarmType alarmType = InputService.EnterAlarmType();
            int priority = InputService.EnterPriority();
            double limit = InputService.EnterDouble();
            Alarm alarm = new Alarm
            {
                Id = dbManagerProxy.GetAITags().Count() + 1,
                Tag = ai,
                Limit = limit,
                Priority = priority,
                Type = alarmType,
            };
            if (dbManagerProxy.AddAlarm(alarm))
                Console.WriteLine("Suceessfully added alarm");
            else
                Console.WriteLine("Alarm addition failed");
        }

        private static void RemoveAlarm()
        {
            Console.WriteLine(dbManagerProxy.GetAlarms());
            Console.WriteLine("Enter id: ");
            string id = Console.ReadLine(); 
            if (dbManagerProxy.RemoveAlarm(id))
                Console.WriteLine("Suceessfully removed alarm");
            else
                Console.WriteLine("Alarm removal failed");
        }

        private static void RegisterUser()
        {
            Console.WriteLine("Username");
            string username = Console.ReadLine();
            Console.WriteLine("Password");
            string password = Console.ReadLine();
            if (authProxy.Registration(username, password))
            {
                Console.WriteLine("Successfully registered new user");
            }
            else
            {
                Console.WriteLine("Something went wrong");
            }
        }

        private static void RemoveTags()
        {
            Console.WriteLine(dbManagerProxy.GetAllTags());
            Console.WriteLine("Enter tag name of tag you want to delete");
            string id = Console.ReadLine();
            if (dbManagerProxy.RemoveTag(id))
                Console.WriteLine("Successfully removed tag");
            else
                Console.WriteLine("Inable to remove tag");
        }

        private static void CreateTag()
        {
            Console.WriteLine("Choose tag type:");
            Console.WriteLine("1) Analog input");
            Console.WriteLine("2) Analog output");
            Console.WriteLine("3) Digital input");
            Console.WriteLine("4) Digital output");
            string inputTag = Console.ReadLine();

            Console.WriteLine("Tag name:");
            string tagName = Console.ReadLine();
            Console.WriteLine("Description:");
            string description = Console.ReadLine();
            Console.WriteLine("I/O address:");
            string ioAddress = Console.ReadLine();

            switch (inputTag)
            {
                case "1":
                    CreateAI(tagName, description, ioAddress);
                    break;
                case "2":
                    CreateAO(tagName, description, ioAddress);
                    break;
                case "3":
                    CreateDI(tagName, description, ioAddress);
                    break;
                case "4":
                    CreateDO(tagName, description, ioAddress);
                    break;
                default:
                    break;
            }
        }

        private static void CreateDO(string tagName, string description, string ioAddress)
        {
            Console.WriteLine("Initial value:");
            double initialValue = InputService.EnterDouble();
            DO digitalOutput = new DO
            {
                Description = description,
                InitialValue = initialValue,
                IOaddress = ioAddress,
                TagName = tagName
            };
            if (dbManagerProxy.AddDOTag(digitalOutput))
                Console.WriteLine("Successfully created DO tag");
            else
                Console.WriteLine("Unable to create DO tag");
        }

        private static void CreateDI(string tagName, string description, string ioAddress)
        {
            Console.WriteLine("Driver:");
            String driver = Console.ReadLine();
            DriverType driverType = driver == "Simulation" ? DriverType.Simulation : DriverType.RealTime;
            Console.WriteLine("Scan time:");
            int scanTime = (int)InputService.EnterDouble();
            Console.WriteLine("On/off scan:");
            bool isActiveScan = Console.ReadLine() == "on" ? true : false;

            DI di = new DI
            {
                TagName = tagName,
                IOaddress = ioAddress,
                Description = description,
                Driver = driverType,
                IsActiveScan = isActiveScan,
                ScanTime = scanTime
            };
            if (dbManagerProxy.AddDITag(di))
                Console.WriteLine("Successfully created DI tag");
            else
                Console.WriteLine("Unable to create AI tag");

        }

        private static void CreateAO(string tagName, string description, string ioAddress)
        {
            Console.WriteLine("Initial value:");
            double initialValue = InputService.EnterDouble();
            Console.WriteLine("Low limit:");
            double lowLimit = InputService.EnterDouble();
            Console.WriteLine("High limit:");
            double highLimit = InputService.EnterDouble();
            Console.WriteLine("Units:");
            string units = Console.ReadLine();

            AO ao = new AO
            {
                Description = description,
                HighLimit = highLimit,
                LowLimit = lowLimit,
                InitialValue = initialValue,
                IOaddress = ioAddress,
                TagName = tagName,
                Units = units
            };
            if (dbManagerProxy.AddAOTag(ao))
                Console.WriteLine("Successfully created AO tag");
            else
                Console.WriteLine("Unable to create AI tag");
        }

        private static void CreateAI(string tagName, string description, string ioAddress)
        {
            Console.WriteLine("Driver:");
            string driver = Console.ReadLine();
            DriverType driverType = driver == "Simulation" ? DriverType.Simulation : DriverType.RealTime;
            Console.WriteLine("Scan time:");
            int scanTime = Int32.Parse(Console.ReadLine());
            Console.WriteLine("on/off scan:");
            bool isActiveScan = Console.ReadLine() == "on" ? true : false;
            Console.WriteLine("Low limit:");
            double lowLimit = InputService.EnterDouble();
            Console.WriteLine("High limit:");
            double highLimit = InputService.EnterDouble();
            Console.WriteLine("Units:");
            string units = Console.ReadLine();
            AI ainput = new AI
            {
                TagName = tagName,
                Description = description,
                IOaddress = ioAddress,
                TagType = TagType.AI,
                ScanTime = scanTime,
                IsActiveScan = isActiveScan,
                Driver = driverType,
                LowLimit = lowLimit,
                HighLimit = highLimit,
                Units = units
            };
            if (dbManagerProxy.AddAITag(ainput))
                Console.WriteLine("Successfully created AI tag");
            else
                Console.WriteLine("Unable to create AI tag");
        }

        private static void TurnScanOff()
        {
            Console.WriteLine(dbManagerProxy.GetInputTags());
            Console.WriteLine("Enter tag name of input tag you want to turn off");
            string id = Console.ReadLine();
            if (dbManagerProxy.ChangeScanStatus(id, false))
            {
                Console.WriteLine("Value changed successfully");
            }
            else
            {
                Console.WriteLine("Invalid tag");
            }
            Console.WriteLine(dbManagerProxy.GetInputTags());
        }

        private static void TurnScanOn()
        {
            Console.WriteLine(dbManagerProxy.GetInputTags());
            Console.WriteLine("Enter tag name of input tag you want to turn on");
            string id = Console.ReadLine();
            if (dbManagerProxy.ChangeScanStatus(id, true))
            {
                Console.WriteLine("Value changed successfully");
            }
            else
            {
                Console.WriteLine("Invalid tag");
            }
            Console.WriteLine(dbManagerProxy.GetInputTags());
        }

        private static void ChangeOutputValue()
        {
            Console.WriteLine("Enter tag name of output tag you want to edit value");
            string id = Console.ReadLine();
            Console.WriteLine("Enter value");
            double value = InputService.EnterDouble();
            if (dbManagerProxy.ChangeOutputValue(id, value))
            {
                Console.WriteLine("Successfully changed value");
            }
            else
            {
                Console.WriteLine("Something went wrong");
            }
        }

        private static void LoginMenu()
        {
            while (true)
            {
                Console.WriteLine("LOGIN");
                Console.WriteLine("Username");
                string username = Console.ReadLine();
                Console.WriteLine("Password");
                string password = Console.ReadLine();

                token = authProxy.Login(username, password);
                   
                if (token != "Login failed")
                {
                    Console.WriteLine("=======================================");
                    return;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("User with this username and password doesn't exist");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("=======================================");
            }
        }
    }
}
