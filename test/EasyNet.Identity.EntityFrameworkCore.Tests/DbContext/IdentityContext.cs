using EasyNet.Identity.EntityFrameworkCore.DbContext;
using EasyNet.Identity.EntityFrameworkCore.Tests.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EasyNet.Identity.EntityFrameworkCore.Tests.DbContext
{
    public class IdentityContext : EasyNetIdentityDbContext<User, Role, int>
    {
        public IdentityContext(DbContextOptions options) : base(options)
        {
        }
    }
}
