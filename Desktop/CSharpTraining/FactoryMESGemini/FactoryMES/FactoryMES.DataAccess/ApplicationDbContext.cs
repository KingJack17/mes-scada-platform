using FactoryMES.Core;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryMES.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<BomItem> BomItems { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<KanbanSignal> KanbanSignals { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<MachineAlarmLog> MachineAlarmLogs { get; set; }
        public DbSet<MachineStatusLog> MachineStatusLogs { get; set; }
        public DbSet<MachineType> MachineTypes { get; set; }
        public DbSet<MaintenanceActivity> MaintenanceActivities { get; set; }
        public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }
        public DbSet<ParameterDefinition> ParameterDefinitions { get; set; }
        public DbSet<Process> Processes { get; set; }
        public DbSet<ProcessDataLog> ProcessDataLogs { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductionData> ProductionData { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<StockLocation> StockLocations { get; set; }
        public DbSet<TraceableUnit> TraceableUnits { get; set; }
        public DbSet<TraceableUnitHistory> TraceableUnitHistories { get; set; }
        public DbSet<UnitOfMeasure> UnitOfMeasures { get; set; }
        public DbSet<UsedPartLog> UsedPartLogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<BomItem>()
                .HasOne(b => b.ParentProduct)
                .WithMany(p => p.BomParentItems)
                .HasForeignKey(b => b.ParentProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BomItem>()
                .HasOne(b => b.ComponentProduct)
                .WithMany(p => p.BomComponentItems)
                .HasForeignKey(b => b.ComponentProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}