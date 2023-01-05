using Data.Models;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Extensions;
using Duende.IdentityServer.EntityFramework.Interfaces;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using Shared;

namespace Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<Employee, Role, string, EmployeeClaim, EmployeeRole, EmployeeLogin, RoleClaim, EmployeeToken>, IPersistedGrantDbContext
    {
        readonly IOptions<OperationalStoreOptions> _storeOptions;

        public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> storeOptions) : base(options)
        {
            _storeOptions = storeOptions;
        }

        public virtual DbSet<Employee>? Employees { get; set; }
        public virtual DbSet<EmployeeRole>? EmployeeRoles { get; set; }
        public virtual DbSet<EmployeeClaim>? EmployeeClaims { get; set; }
        public virtual DbSet<EmployeeLogin>? EmployeeLogins { get; set; }
        public virtual DbSet<EmployeeToken>? EmployeeTokens { get; set; }
        
        public DbSet<Key>? Keys { get; set; }
        public DbSet<PersistedGrant>? PersistedGrants { get; set; }        
        public DbSet<DeviceFlowCodes>? DeviceFlowCodes { get; set; }

        public Action? ChangeTrackerCleared { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigurePersistedGrantContext(_storeOptions.Value);

            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new RoleClaimConfiguration());

            builder.ApplyConfiguration(new EmployeeConfiguration());
            builder.ApplyConfiguration(new EmployeeRoleConfiguration());
            builder.ApplyConfiguration(new EmployeeClaimConfiguration());
            builder.ApplyConfiguration(new EmployeeTokenConfiguration());
            builder.ApplyConfiguration(new EmployeeLoginConfiguration());
        }       

        Task<int> IPersistedGrantDbContext.SaveChangesAsync() => base.SaveChangesAsync();

        public override async Task<int> SaveChangesAsync(CancellationToken token = default!)
        {
            ChangeTracker.DetectChanges();

            var markedAsDeleted = ChangeTracker
                .Entries()
                .Where(x => x.State == EntityState.Deleted);

            foreach (var item in markedAsDeleted)
            {
                if (item.Entity is DataModelBase entity)
                {
                    // Set the entity to unchanged (if we mark the whole entity as Modified, every field gets sent to Db as an update)
                    item.State = EntityState.Unchanged;
                    // Only update the IsDeleted flag - only this will get sent to the Db
                    entity.OnDelete();
                }
            }
            return await base.SaveChangesAsync(token);
        }

        public void ClearChangeTracker()
        {
            ChangeTracker.Clear();

            ChangeTrackerCleared?.Invoke();
        }
    }

    class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> b)
        {
            b.ToTable("Roles");

            b.HasMany(a => a.RoleClaims)
                .WithOne(b => b.Role)
                .HasForeignKey(a => a.RoleId);

            b.Navigation(a => a.RoleClaims)
               .AutoInclude();
        }
    }
    class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
    {
        public void Configure(EntityTypeBuilder<RoleClaim> b)
        {
            b.ToTable("RoleClaims");

            b.HasOne(a => a.Role)
                .WithMany(b => b.RoleClaims)
                .HasForeignKey(a => a.RoleId);

            b.Navigation(a => a.Role)
               .AutoInclude();
        }
    }

    class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> b)
        {
            b.ToTable("Employees");

            b.HasMany(a => a.EmployeeRoles)
                .WithOne(b => b.Employee)
                .HasForeignKey(a => a.UserId);

            b.HasMany(a => a.EmployeeClaims)
                .WithOne(b => b.Employee);

            b.HasMany(a => a.EmployeeTokens)
                .WithOne(b => b.Employee)
                .HasForeignKey(a => a.UserId);

            b.HasMany(a => a.EmployeeLogins)
                .WithOne(b => b.Employee)
                .HasForeignKey(a => a.UserId);

            b.Navigation(a => a.EmployeeRoles)
               .AutoInclude();
        }
    }
    class EmployeeRoleConfiguration : IEntityTypeConfiguration<EmployeeRole>
    {
        public void Configure(EntityTypeBuilder<EmployeeRole> b)
        {
            b.HasKey(r => new { r.UserId, r.RoleId });

            b.ToTable("EmployeeRoles");

            b.HasOne(a => a.Employee)
                .WithMany(b => b.EmployeeRoles)
                .HasForeignKey(a => a.UserId);

            b.HasOne(a => a.Role)
                .WithMany(b => b.EmployeeRoles)
                .HasForeignKey(a => a.RoleId);

            b.Navigation(a => a.Role)
               .AutoInclude();

            b.Navigation(a => a.Employee)
              .AutoInclude();
        }
    }
    class EmployeeClaimConfiguration : IEntityTypeConfiguration<EmployeeClaim>
    {
        public void Configure(EntityTypeBuilder<EmployeeClaim> b)
        {
            b.ToTable("EmployeeClaims");

            b.HasOne(a => a.Employee)
                .WithMany(b => b.EmployeeClaims)
                .HasForeignKey(a => a.UserId);
        }
    }
    class EmployeeTokenConfiguration : IEntityTypeConfiguration<EmployeeToken>
    {
        public void Configure(EntityTypeBuilder<EmployeeToken> b)
        {
            b.ToTable("EmployeeTokens");

            b.HasOne(a => a.Employee)
                .WithMany(b => b.EmployeeTokens)
                .HasForeignKey(a => a.UserId);

            b.Navigation(a => a.Employee)
                .AutoInclude();
        }
    }
    class EmployeeLoginConfiguration : IEntityTypeConfiguration<EmployeeLogin>
    {
        public void Configure(EntityTypeBuilder<EmployeeLogin> b)
        {
            b.ToTable("EmployeeLogins");

            b.HasOne(a => a.Employee)
                .WithMany(b => b.EmployeeLogins)
                .HasForeignKey(a => a.UserId);

            b.Navigation(a => a.Employee)
                .AutoInclude();
        }
    }
}