using OPIDEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MSM.DAL
{
    public class OPIDDB : DbContext
    {
        public OPIDDB()
            :base("OPIDEntities")
        {
        }

        public DbSet<UnresolvedCheck> UnresolvedChecks { get; set; }
    }
}