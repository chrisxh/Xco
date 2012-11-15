using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Xco.Models
{
    public class XcoContext : DbContext
    {
                public XcoContext() : base("name=XcoContext")
        {
        }

        public DbSet<Link> Links { get; set; }

        public DbSet<Visit> Visits { get; set; }
    }
}