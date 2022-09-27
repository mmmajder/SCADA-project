using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TrendingApp.ServiceReference1;

namespace TrendingApp
{
    public class Callback : ServiceReference1.ITrendingAppServiceCallback
    {
        public void ValueChanged(TagInvoked[] tags)
        {
            Console.Clear();
            Console.WriteLine("Input tag values");
            Console.WriteLine("Tag name \t TimeStamp \t Value");
            foreach (TagInvoked ti in tags)
            {
                Console.WriteLine(ti.TagName + " \t" + ti.TimeStamp + " \t" + ti.Value);
            }
        }
    }

    class Program
    {
        public static InstanceContext ic = new InstanceContext(new Callback());
        public static ServiceReference1.TrendingAppServiceClient trendingApp = new ServiceReference1.TrendingAppServiceClient(ic);
        static void Main(string[] args)
        {
            trendingApp.Init();
            while (true)
            {

            }
        }
    }
}
