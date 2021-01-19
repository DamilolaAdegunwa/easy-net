using System.Collections.Generic;
using System.Threading.Tasks;
using EasyNet.Dto;

namespace EasyNet.ApplicationService
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>

    public interface IEasyNetQueryAppService<TEntityDto, in TPrimaryKey, in TGetAllInput> : IEasyNetAppService
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        /// <summary>
        /// Get a entity as dto.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<TEntityDto> GetAsync(IEntityDto<TPrimaryKey> input);

        /// <summary>
        /// Get all entities as dto list.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<TEntityDto>> GetAllAsync(TGetAllInput input);
    }
}
