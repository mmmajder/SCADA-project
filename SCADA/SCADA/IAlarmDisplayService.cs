﻿using SCADA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SCADA
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAlarmDisplayService" in both code and config file together.
    [ServiceContract(CallbackContract =typeof(IAlarmDisplayServiceCallback))]
    public interface IAlarmDisplayService
    {
        [OperationContract]
        void Init();
    }

    public interface IAlarmDisplayServiceCallback
    {
        [OperationContract]
        void FireAlarm(AlarmInvoked alarmInvoked);
    }

}
