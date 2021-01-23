using System.Threading.Tasks;
using EasyNet.Dto;

namespace EasyNet.ApplicationService
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public interface IEasyNetCrudAndPassivableAppService<TEntityDto, in TPrimaryKey, in TGetAllInput, in TCreateInput> : IEasyNetCrudAndPassivableAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TEntityDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
    }

    public interface IEasyNetCrudAndPassivableAppService<TEntityDto, in TPrimaryKey, in TGetAllInput> : IEasyNetCrudAndPassivableAppService<TEntityDto, TPrimaryKey, TGetAllInput, TEntityDto, TEntityDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
    }

    public interface IEasyNetCrudAndPassivableAppService<TEntityDto, in TPrimaryKey, in TGetAllInput, in TCreateInput, in TUpdateInput> : IEasyNetCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        /// <summary>
        /// Archive a entity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task ArchiveAsync(TPrimaryKey id);

        /// <summary>
        /// Reactive a entity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task ActivateAsync(TPrimaryKey id);
    }
}
