using SCADA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SCADA
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IReportManagerService" in both code and config file together.
    [ServiceContract]
    public interface IReportManagerService
    {
        [OperationContract]
        List<AlarmInvoked> AlarmsInSameTimePeriod(DateTime start, DateTime end);
        [OperationContract]
        List<AlarmInvoked> AlarmsByPriority(int priority);
        [OperationContract]
        List<TagInvoked> TagInvokecInSameTimePeriod(DateTime start, DateTime end);
        [OperationContract]
        List<TagInvoked> LastValueOfAITags();
        [OperationContract]
        List<TagInvoked> LastValueOfDITags();
        [OperationContract]
        List<TagInvoked> AllValuesOfTag(string tagName);
    }
}
