using FScan.Reports.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Infrastructure.Data
{
    public class FScanContext :DbContext
    {
        public FScanContext(DbContextOptions options) : base(options) { }

        public DbSet<NGAC_AUTHLOG> NGAC_AUTHLOG { get; set; }
        public DbSet<NGAC_GROUP> NGAC_GROUP { get; set; }

        public DbSet<NGAC_USERINFO> NGAC_USERINFO { get; set; }
        public DbSet<NGAC_GROUPTERMINAL> NGAC_GROUPTERMINAL { get; set; }
        public DbSet<NGAC_TERMINAL> NGAC_TERMINAL { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FScanContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

    }
}
