using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyNet.Domain.Entities;
using EasyNet.Domain.Entities.Auditing;
using EasyNet.EntityFrameworkCore.Extensions;
using EasyNet.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EasyNet.EntityFrameworkCore
{
    public class EasyNetDbContext : DbContext
    {
        public EasyNetDbContext(DbContextOptions options, IEasyNetSession session = null) : base(options)
        {
            Session = session ?? NullEasyNetSession.Instance;
        }

        protected IEasyNetSession Session { get; }

        public override int SaveChanges()
        {
            ApplyEasyNetConcepts();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            ApplyEasyNetConcepts();

            return base.SaveChangesAsync(cancellationToken);
        }

        protected virtual void ApplyEasyNetConcepts()
        {
            foreach (var entry in ChangeTracker.Entries().ToList())
            {
                if (entry.State != EntityState.Modified && entry.CheckOwnedEntityChange())
                {
                    Entry(entry.Entity).State = EntityState.Modified;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        ApplyAbpConceptsForAddedEntity(entry);
                        break;
                    case EntityState.Modified:
                        ApplyAbpConceptsForModifiedEntity(entry);
                        break;
                    case EntityState.Deleted:
                        ApplyAbpConceptsForDeletedEntity(entry);
                        break;
                }
            }
        }

        protected virtual void ApplyAbpConceptsForAddedEntity(EntityEntry entry)
        {
            EntityAuditingHelper.SetCreationAuditProperties(entry.Entity, Session.UserId);
        }

        protected virtual void ApplyAbpConceptsForModifiedEntity(EntityEntry entry)
        {
            EntityAuditingHelper.SetModificationAuditProperties(entry.Entity, Session.UserId);
        }

        protected virtual void ApplyAbpConceptsForDeletedEntity(EntityEntry entry)
        {
            if (entry.Entity is ISoftDelete)
            {
                entry.State = EntityState.Modified;
                EntityAuditingHelper.SetDeletionAuditProperties(entry.Entity, Session.UserId);
            }
        }
    }
}
