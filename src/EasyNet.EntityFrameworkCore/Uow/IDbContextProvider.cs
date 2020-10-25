using Microsoft.EntityFrameworkCore;

namespace EasyNet.EntityFrameworkCore.Uow
{
    public interface IDbContextProvider<out TDbContext>
        where TDbContext : DbContext
    {
        TDbContext GetDbContext();
    }
}
