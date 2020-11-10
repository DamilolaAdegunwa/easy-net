using EasyNet.Domain.Uow;
using EasyNet.EntityFrameworkCore.Tests.Entities;
using EasyNet.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EasyNet.EntityFrameworkCore.Tests.DbContext
{
    public class EfCoreContext : EasyNetDbContext
    {
        public EfCoreContext(DbContextOptions options, ICurrentUnitOfWorkProvider currentUnitOfWorkProvider = null, IEasyNetSession session = null) : base(options, currentUnitOfWorkProvider, session)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<TestCreationAudited> TestCreationAudited { get; set; }

        public virtual DbSet<TestModificationAudited> TestModificationAudited { get; set; }

        public virtual DbSet<TestDeletionAudited> TestDeletionAudited { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder =>
            {
                builder.AddDebug();
            }));
        }
    }
}
