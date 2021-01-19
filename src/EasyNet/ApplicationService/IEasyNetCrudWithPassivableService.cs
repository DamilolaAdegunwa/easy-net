using System.Threading.Tasks;
using EasyNet.Dto;

namespace EasyNet.ApplicationService
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public interface IEasyNetCrudWithPassivableService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput> : IEasyNetCrudWithPassivableService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TEntityDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
    }

    public interface IEasyNetCrudWithPassivableService<TEntityDto, TPrimaryKey, in TGetAllInput> : IEasyNetCrudWithPassivableService<TEntityDto, TPrimaryKey, TGetAllInput, TEntityDto, TEntityDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
    }

    public interface IEasyNetCrudWithPassivableService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput, in TUpdateInput> : IEasyNetCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        /// <summary>
        /// Archive a entity.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ArchiveAsync(IEntityDto<TPrimaryKey> input);

        /// <summary>
        /// Reactive a entity.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ActivateAsync(IEntityDto<TPrimaryKey> input);
    }
}
