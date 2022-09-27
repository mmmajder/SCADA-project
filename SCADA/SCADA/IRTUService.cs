using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SCADA
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRTUService" in both code and config file together.
    [ServiceContract]
    public interface IRTUService
    {
        [OperationContract (IsInitiating = true)]
        int Init(string address);
        [OperationContract]
        bool SendValue(byte[] signature, int id, double value);
        [OperationContract]
        string Get(int id);
    }
}
