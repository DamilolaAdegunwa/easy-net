using Microsoft.EntityFrameworkCore;

namespace EasyNet.EntityFrameworkCore.Repositories
{
    public interface IRepositoryWithDbContext
    {
        /// <summary>
        /// Gets <see cref="DbContext"/>
        /// </summary>
        /// <returns></returns>
        DbContext GetDbContext();
    }
}
