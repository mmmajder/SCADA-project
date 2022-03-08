using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RTU
{
    class Program
    {
        public static ServiceReference1.RTUServiceClient rtuSevice = new ServiceReference1.RTUServiceClient();

        private static CspParameters csp;
        private static RSACryptoServiceProvider rsa;

        public static void CreateAsmKeys()
        {
            csp = new CspParameters();
            rsa = new RSACryptoServiceProvider(csp);
        }

        private static byte[] SignMessage(string message)
        {
            using (SHA256 sha = SHA256Managed.Create())
            {
                var hashValue = sha.ComputeHash(Encoding.UTF8.GetBytes(message));
                var formatter = new RSAPKCS1SignatureFormatter(rsa);
                formatter.SetHashAlgorithm("SHA256");
                return formatter.CreateSignature(hashValue);
            }
        }

        const string EXPORT_FOLDER = @"C:\public_key\";
        
        private static void ExportPublicKey(string fileName)
        {
            //Kreiranje foldera za eksport ukoliko on ne postoji
            if (!(Directory.Exists(EXPORT_FOLDER)))
                Directory.CreateDirectory(EXPORT_FOLDER);
            string path = Path.Combine(EXPORT_FOLDER, fileName);
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.Write(rsa.ToXmlString(false));
            }
        }

        private static int InputNumber()
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out var parsedNumber))
                {
                    return parsedNumber;
                }
                Console.WriteLine("Warning! Enter number!");
            }
        }
        
        public static double GetNewValue(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }


        static void Main(string[] args)
        {
            string address;
            List<string> simulationDriverAddresses = new List<string> { "S", "C", "R" };
            while (true)
            {
                Console.WriteLine("Enter address of RTU");
                address = Console.ReadLine();
                if (!simulationDriverAddresses.Contains(address))
                {
                    break;
                }
                Console.WriteLine("You should not use S, C or R");
            }

            int id = rtuSevice.Init(address);
            Console.WriteLine(id);

            Console.WriteLine(rtuSevice.Get(id)); 



            if (id!=-1)
            {
                Console.WriteLine("Enter minimum: ");
                int min = InputNumber();
                Console.WriteLine("Enter maximum: ");
                int max = InputNumber();
                Console.WriteLine("Enter sleep time: ");
                int sleepTime = (int) InputNumber();

                CreateAsmKeys();
                ExportPublicKey(id.ToString());

                while (true)
                {
                    Thread.Sleep(sleepTime * 1000);
                    double value = GetNewValue(min, max);
                    byte[] signature = SignMessage(value.ToString());
                    rtuSevice.SendValue(signature, id, value);
                    Console.Write("Value sent: " + value + "\n");

                }
            }
        }
    }
}
