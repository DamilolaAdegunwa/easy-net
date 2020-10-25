using Microsoft.EntityFrameworkCore;

namespace EasyNet.EntityFrameworkCore
{
    public class EasyNetDbContext : DbContext
    {
        public EasyNetDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
