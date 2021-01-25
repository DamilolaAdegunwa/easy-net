using System.Collections.Generic;
using System.Threading.Tasks;
using EasyNet.Dto;

namespace EasyNet.ApplicationService
{
    public interface IEasyNetQueryAppService<TEntityDto, in TGetAllInput> : IEasyNetQueryAppService<TEntityDto, int, TGetAllInput>
        where TEntityDto : IEntityDto<int>
    {
    }

    public interface IEasyNetQueryAppService<TEntityDto, in TPrimaryKey, in TGetAllInput> : IEasyNetAppService
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        Task<TEntityDto> GetAsync(TPrimaryKey id);

        Task<List<TEntityDto>> GetAllAsync(TGetAllInput input);
    }
}
