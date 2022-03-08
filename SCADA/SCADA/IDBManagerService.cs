using SCADA.Model.Alarms;
using SCADA.Model.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SCADA
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDBManagerService" in both code and config file together.
    [ServiceContract]
    public interface IDBManagerService
    {
        [OperationContract]
        bool ChangeOutputValue(string tagName, double value);

        [OperationContract]
        bool ChangeScanStatus(string tagName, bool status);

        [OperationContract]
        string GetInputTags();

        [OperationContract]
        bool AddAITag(AI ai);
        [OperationContract]
        bool AddAOTag(AO ao);
        [OperationContract]
        bool AddDITag(DI di);
        [OperationContract]
        bool AddDOTag(DO digitalOutput);

        [OperationContract]
        string GetOutputValues();

        [OperationContract]
        string GetAllTags();

        [OperationContract]
        bool RemoveTag(string id);
        [OperationContract]
        string GetAITags();
        [OperationContract]
        AI GetAIById(string id);
        [OperationContract]
        bool AddAlarm(Alarm alarm);
        [OperationContract]
        bool RemoveAlarm(string id);
        [OperationContract]
        string GetAlarms();

    }
}
