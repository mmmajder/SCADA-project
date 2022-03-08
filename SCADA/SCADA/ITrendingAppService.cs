using SCADA.Model;
using SCADA.Model.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SCADA
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITrendingAppService" in both code and config file together.
    [ServiceContract(CallbackContract = typeof(ITrendingAppServiceCallback))]
    public interface ITrendingAppService
    {
        [OperationContract]
        void Init();
    }

    public interface ITrendingAppServiceCallback
    {
        [OperationContract]
        void ValueChanged(List<TagInvoked> inputTags);
    }
}
