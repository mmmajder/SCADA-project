using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCADA.App_Code
{
    public class Init
    {
        public static void AppInitialize()
        {
            TagProcessing.Init();
        }
    }
}