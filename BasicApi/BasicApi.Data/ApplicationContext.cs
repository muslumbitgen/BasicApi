using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BasicApi.Items.Entities;
using BasicApi.Items.Types;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BasicApi.Data
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        private readonly IContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;

        public ApplicationContext(DbContextOptions<ApplicationContext> options,
                                IContextAccessor contextAccessor,
                                IConfiguration configuration) : base(options)
        {
            _contextAccessor = contextAccessor;
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(_configuration.GetValue<string>("db:defaultSchemaName"));

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataIdentifierType).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var addedEntries = ChangeTracker.Entries()
                                            .Where(x => x.State == EntityState.Added)
                                            .ToList();

            addedEntries.ForEach(added =>
            {
                if (added.Entity is EntityBase)
                {
                    added.Property("CreatedAt").CurrentValue = DateTime.Now;
                    added.Property("ModifiedAt").CurrentValue = DateTime.Now;
                }

                if (added.Entity is IAuditableEntityBase)
                {
                    added.Property("CreatedBy").CurrentValue = _contextAccessor.UserId;
                    added.Property("ModifiedBy").CurrentValue = _contextAccessor.UserId;
                }
            });

            var updatedEntries = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified).ToList();

            updatedEntries.ForEach(added =>
            {
                if (added.Entity is EntityBase)
                {
                    added.Property("ModifiedAt").CurrentValue = DateTime.Now;
                }

                if (added.Entity is IAuditableEntityBase)
                {
                    added.Property("ModifiedBy").CurrentValue = _contextAccessor.UserId;
                }
            });

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
