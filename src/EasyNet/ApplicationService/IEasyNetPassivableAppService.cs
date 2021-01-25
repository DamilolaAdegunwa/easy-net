using System.Threading.Tasks;

namespace EasyNet.ApplicationService
{
    public interface IEasyNetPassivableAppService : IEasyNetPassivableAppService<int>
    {
    }

    public interface IEasyNetPassivableAppService<in TPrimaryKey> : IEasyNetAppService
    {
        Task ArchiveAsync(TPrimaryKey id);

        Task ActivateAsync(TPrimaryKey id);
    }
}
