using Newspaper.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Newspaper.Models.DAL
{
    public class NewspaperEntities : DbContext
        {
            public NewspaperEntities()
                : base("Newspaper")
            {
            }
            public DbSet<Admin> Admin { get; set; }

        public DbSet<Customer> Customer { get; set; }

        public DbSet<SalesMan> SalesMan { get; set; }

        public DbSet<Service> Service { get; set; }

        public DbSet<AreaRegister> AreaRegister { get; set; }

        public DbSet<ActivityLog> ActivityLog { get; set; }

        public DbSet<Branch> Branch { get; set; }
        public DbSet<DaywisePaperDispatch> DayWisePaperDispatch{ get; set; }
        public DbSet<HoldingDays> HoldingDays { get; set; }
        public DbSet<Customercoun> Customercoun { get; set; }
        public DbSet<Newspaper> Newspaper { get; set; }
        public DbSet<ServiceAssign> ServiceAssign { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<PrakashanGroup> prakashanGroups { get; set; }
        public DbSet<Prakashanreport> prakashanreports { get; set; }
        public DbSet<GroupName> groupNames { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<RouteReport> RouteReports { get; set; }
        public DbSet<FiscalYear> FiscalYear { get; set; }


    }
}