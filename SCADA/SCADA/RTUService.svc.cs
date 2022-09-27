using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace SCADA
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RTUService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RTUService.svc or RTUService.svc.cs at the Solution Explorer and start debugging.
    public class RTUService : IRTUService
    {
        static Dictionary<int, string> Adresses = new Dictionary<int, string>();
        static int NumberOfRTUAddresses = 0;
        static readonly object addressLocker = new object();
        const string IMPORT_FOLDER = @"C:\public_key\";
        private static CspParameters csp;
        private static RSACryptoServiceProvider rsa;

        private static void ImportPublicKey(string fileName)
        {
            string path = Path.Combine(IMPORT_FOLDER, fileName);
            //Provera da li fajl sa javnim ključem postoji na prosleđenoj lokaciji
            FileInfo fi = new FileInfo(path);
            if (fi.Exists)
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    csp = new CspParameters();
                    rsa = new RSACryptoServiceProvider(csp);
                    string publicKeyText = reader.ReadToEnd();
                    rsa.FromXmlString(publicKeyText);
                }
            }
        }

        private static bool VerifySignedMessage(string message, byte[] signature)
        {
            using (SHA256 sha = SHA256Managed.Create())
            {
                var hashValue = sha.ComputeHash(Encoding.UTF8.GetBytes(message));
                var deformatter = new RSAPKCS1SignatureDeformatter(rsa);
                deformatter.SetHashAlgorithm("SHA256");
                return deformatter.VerifySignature(hashValue, signature);
            }
        }


        public int Init(string address)
        {
            if (RTD.AddAddress(address))
            {
                int id;
                lock (addressLocker)
                {
                    id = NumberOfRTUAddresses;
                    NumberOfRTUAddresses++;
                    Adresses[id] = address;
                    
                }
                return id;
            }
            return -1;
        }

        public string Get(int id)
        {
            return Adresses[id];
        }

        public bool SendValue(byte[] signature, int id, double value)
        {
            ImportPublicKey(id.ToString());
            if (VerifySignedMessage(value.ToString(), signature))
            {
                RTD.AddNewValue(Adresses[id], value);
                return true;
            }
            return false;
        }
    }
}
