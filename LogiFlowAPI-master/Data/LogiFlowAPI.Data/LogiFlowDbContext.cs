namespace LogiFlowAPI.Data
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using LogiFlowAPI.Data.Common.Models;
    using LogiFlowAPI.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class LogiFlowDbContext : IdentityDbContext<User, Role, string>
    {
        private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
            typeof(LogiFlowDbContext).GetMethod(
                nameof(SetIsDeletedQueryFilter),
                BindingFlags.NonPublic | BindingFlags.Static);

        public LogiFlowDbContext(DbContextOptions<LogiFlowDbContext> options)
            : base(options)
        {
        }

        public DbSet<Batch> Batches { get; set; }

        public DbSet<Delivery> Deliveries { get; set; }

        public DbSet<DeliveryStatus> DeliveryStatuses { get; set; }

        public DbSet<Invite> Invites { get; set; }

        public DbSet<InviteStatus> InviteStatuses { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductTag> ProductTags { get; set; }

        public DbSet<Shipment> Shipments { get; set; }

        public DbSet<ShippedProduct> ShippedProducts { get; set; }

        public DbSet<ShippedProductTag> ShippedProductTags { get; set; }

        public DbSet<StorageArea> StorageAreas { get; set; }

        public DbSet<StorageAreaTag> StorageAreaTags { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Warehouse> Warehouses { get; set; }

        public override int SaveChanges()
        {
            return this.SaveChanges(true);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return this.SaveChangesAsync(true, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Needed for Identity models configuration
            base.OnModelCreating(builder);

            this.ChangeDefaultIdentityTableNames(builder);

            this.ConfigureUserIdentityRelations(builder);

            EntityIndexesConfiguration.Configure(builder);

            var entityTypes = builder.Model.GetEntityTypes().ToList();

            // Set global query filter for not deleted entities only
            var deletableEntityTypes = entityTypes
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
            foreach (var deletableEntityType in deletableEntityTypes)
            {
                var method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(deletableEntityType.ClrType);
                method.Invoke(null, new object[] { builder });
            }

            // Disable cascade delete
            var foreignKeys = entityTypes
                .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        private static void SetIsDeletedQueryFilter<T>(ModelBuilder builder)
            where T : class, IDeletableEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        private void ChangeDefaultIdentityTableNames(ModelBuilder builder)
        {
            builder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
            });
            builder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }

        // Applies configurations
        private void ConfigureUserIdentityRelations(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }

        private void ApplyAuditInfoRules()
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IAuditInfo &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}
