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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TrendingAppService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TrendingAppService.svc or TrendingAppService.svc.cs at the Solution Explorer and start debugging.
    public class TrendingAppService : ITrendingAppService
    {
        static readonly object locker = new object();
        public static int numberOfInstances = 0;
        public static Dictionary<int, ITrendingAppServiceCallback> proxies = new Dictionary<int, ITrendingAppServiceCallback>();

        public ITrendingAppServiceCallback Proxy
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<ITrendingAppServiceCallback>();
            }
        }


        public void Init()
        {
            lock(locker)
            {
                proxies[numberOfInstances] = Proxy;
                numberOfInstances++;
            }
        }

        public static void UpdateTrendingApp(List<TagInvoked> invokedTags)
        {
            foreach (int key in proxies.Keys)
            {
                proxies[key].ValueChanged(invokedTags);
            }
        }
    }
}
