using SCADA.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SCADA.Data
{
    public class AlarmInvokedContext : DbContext
    {
        public DbSet<AlarmInvoked> AlarmInvokes { get; set; }
    }
}