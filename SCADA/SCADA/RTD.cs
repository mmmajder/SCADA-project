using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCADA
{
    public class RTD
    {
        //private static Dictionary<string, >
        static readonly object AddressLocker = new object();
        private static Dictionary<string, double> ValuesOnAddresses = new Dictionary<string, double>();

        public static bool AddAddress(string address)
        {
            lock (AddressLocker)
            {
                if (!ValuesOnAddresses.ContainsKey(address)) {
                    ValuesOnAddresses[address] = -1;
                    return true;
                }
                return false;
            }
        }

        public static void AddNewValue(string address, double newValue)
        {
            lock (AddressLocker)
            {
                if (ValuesOnAddresses.ContainsKey(address))
                {
                    ValuesOnAddresses[address] = newValue;
                }
                else
                {
                    throw new Exception("Address doesn't exist");
                }
            }
        }

        public static double ReadValue(string address)
        {
            lock (AddressLocker)
            {
                if (ValuesOnAddresses.ContainsKey(address))
                {
                    return ValuesOnAddresses[address];
                }
                else
                {
                    return -1;
                    //throw new Exception("Address doesn't exist");
                }
            }
            
        }


    }
}