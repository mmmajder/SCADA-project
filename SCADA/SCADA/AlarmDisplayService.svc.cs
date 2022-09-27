using SCADA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SCADA
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AlarmDisplayService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AlarmDisplayService.svc or AlarmDisplayService.svc.cs at the Solution Explorer and start debugging.
    public class AlarmDisplayService : IAlarmDisplayService
    {
        private static int NumberOfClients = 0;
        private static Dictionary<int, IAlarmDisplayServiceCallback> callbackChannels = new Dictionary<int, IAlarmDisplayServiceCallback>();
        static readonly object locker = new object();

        public IAlarmDisplayServiceCallback Proxy
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IAlarmDisplayServiceCallback>();
            }
        }

        public void Init()
        {
            lock(locker)
            {
                callbackChannels[NumberOfClients] = Proxy;
                NumberOfClients++;
            }
        }

        public static void ActivateAlarmDisplay(AlarmInvoked alarmInvoked)
        {
            foreach(int id in callbackChannels.Keys)
            {
                callbackChannels[id].FireAlarm(alarmInvoked);
            }
        }
    }
}
