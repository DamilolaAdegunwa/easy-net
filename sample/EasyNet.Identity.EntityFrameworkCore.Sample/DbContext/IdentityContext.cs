using EasyNet.Identity.EntityFrameworkCore.DbContext;
using EasyNet.Identity.EntityFrameworkCore.Sample.Domain;
using Microsoft.EntityFrameworkCore;

namespace EasyNet.Identity.EntityFrameworkCore.Sample.DbContext
{
    public class IdentityContext : EasyNetIdentityDbContext<User>
    {
        public IdentityContext(DbContextOptions options) : base(options)
        {
        }
    }
}
