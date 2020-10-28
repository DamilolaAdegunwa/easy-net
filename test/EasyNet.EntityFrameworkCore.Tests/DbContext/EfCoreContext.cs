using EasyNet.EntityFrameworkCore.Tests.Entities;
using EasyNet.Runtime.Session;
using Microsoft.EntityFrameworkCore;

namespace EasyNet.EntityFrameworkCore.Tests.DbContext
{
    public class EfCoreContext : EasyNetDbContext
    {
        public EfCoreContext(DbContextOptions options, IEasyNetSession session = null) : base(options, session)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<TestCreationAudited> TestCreationAudited { get; set; }

        public virtual DbSet<TestModificationAudited> TestModificationAudited { get; set; }

        public virtual DbSet<TestDeletionAudited> TestDeletionAudited { get; set; }
    }
}
