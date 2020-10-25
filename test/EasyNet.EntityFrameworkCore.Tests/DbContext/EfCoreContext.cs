using EasyNet.EntityFrameworkCore.Tests.Entities;
using Microsoft.EntityFrameworkCore;

namespace EasyNet.EntityFrameworkCore.Tests.DbContext
{
    public class EfCoreContext : EasyNetDbContext
    {
        public EfCoreContext(DbContextOptions options) :base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Role> Roles { get; set; }
    }
}
