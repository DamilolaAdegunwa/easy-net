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
        Task<TEntityDto> GetAsync(IEntityDto<TPrimaryKey> input);

        Task<List<TEntityDto>> GetAllAsync(TGetAllInput input);
    }
}
