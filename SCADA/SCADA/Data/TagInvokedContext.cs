using SCADA.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SCADA.Data
{
    public class TagInvokedContext : DbContext
    {
        public DbSet<TagInvoked> InvokedTags { get; set; } 
    }
}