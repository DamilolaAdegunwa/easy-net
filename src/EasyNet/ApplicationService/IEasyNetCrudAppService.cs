using System.Threading.Tasks;
using EasyNet.Dto;

namespace EasyNet.ApplicationService
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public interface IEasyNetCrudAppService<TEntityDto, in TPrimaryKey, in TGetAllInput, in TCreateInput> : IEasyNetCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TEntityDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
    }

    public interface IEasyNetCrudAppService<TEntityDto, in TPrimaryKey, in TGetAllInput> : IEasyNetCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TEntityDto, TEntityDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
    }

    public interface IEasyNetCrudAppService<TEntityDto, in TPrimaryKey, in TGetAllInput, in TCreateInput, in TUpdateInput> : IEasyNetQueryAppService<TEntityDto, TPrimaryKey, TGetAllInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        /// <summary>
        /// Insert a entity.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<TEntityDto> CreateAsync(TCreateInput input);

        /// <summary>
        /// Update a entity.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<TEntityDto> UpdateAsync(TUpdateInput input);

        /// <summary>
        /// Delete a entity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(TPrimaryKey id);
    }
}
