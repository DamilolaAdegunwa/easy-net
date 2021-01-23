using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNet.DependencyInjection;
using EasyNet.Domain.Entities;
using EasyNet.Domain.Repositories;
using EasyNet.Dto;

namespace EasyNet.ApplicationService
{
    public abstract class EasyNetQueryAppService<TEntity, TEntityDto, TGetAllInput> : EasyNetQueryAppService<TEntity, TEntityDto, int, TGetAllInput>
        where TEntity : class, IEntity<int>
        where TEntityDto : IEntityDto<int>
    {
        protected EasyNetQueryAppService(IIocResolver iocResolver, IRepository<TEntity, int> repository) : base(iocResolver, repository)
        {
        }
    }

    /// <summary>
    /// Derive your application services from this class.
    /// </summary>

    public abstract class EasyNetQueryAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput> : EasyNetAppService
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected readonly IRepository<TEntity, TPrimaryKey> Repository;

        protected EasyNetQueryAppService(IIocResolver iocResolver, IRepository<TEntity, TPrimaryKey> repository) : base(iocResolver)
        {
            Repository = repository;
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TEntityDto> GetAsync(TPrimaryKey id)
        {
            var entity = await Repository.GetAsync(id);

            return MapToEntityDto(entity);
        }

        /// <summary>
        /// Get all
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<TEntityDto>> GetAllAsync(TGetAllInput input)
        {
            var entities = await Repository.GetAllListAsync();

            return entities.Select(MapToEntityDto).ToList();
        }

        /// <summary>
        /// Map to entity dto
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual TEntityDto MapToEntityDto(TEntity entity)
        {
            return ObjectMapper.Map<TEntityDto>(entity);
        }
    }
}
