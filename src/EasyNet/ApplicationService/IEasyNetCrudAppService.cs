using System.Threading.Tasks;
using EasyNet.Dto;

namespace EasyNet.ApplicationService
{
    public interface IEasyNetCrudAppService<TEntityDto> : IEasyNetCrudAppService<TEntityDto, int, TEntityDto, TEntityDto>
        where TEntityDto : IEntityDto<int>
    {
    }

    public interface IEasyNetCrudAppService<TEntityDto, in TPrimaryKey> : IEasyNetCrudAppService<TEntityDto, TPrimaryKey, TEntityDto, TEntityDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
    }

    public interface IEasyNetCrudAppService<TEntityDto, in TPrimaryKey, in TCreateInput> : IEasyNetCrudAppService<TEntityDto, TPrimaryKey, TCreateInput, TEntityDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
    }

    public interface IEasyNetCrudAppService<TEntityDto, in TPrimaryKey, in TCreateInput, in TUpdateInput> : IEasyNetAppService
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        Task<TEntityDto> CreateAsync(TCreateInput input);

        Task<TEntityDto> UpdateAsync(TUpdateInput input);

        Task DeleteAsync(TPrimaryKey id);
    }
}
