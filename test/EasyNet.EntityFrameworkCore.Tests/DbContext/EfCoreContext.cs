using EasyNet.EntityFrameworkCore.Tests.Entities;
using Microsoft.EntityFrameworkCore;

namespace EasyNet.EntityFrameworkCore.Tests.DbContext
{
    public class EfCoreContext : EasyNetDbContext
    {
        public EfCoreContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<TestCreationAudited> TestCreationAudited { get; set; }

        public virtual DbSet<TestModificationAudited> TestModificationAudited { get; set; }
    }
}
