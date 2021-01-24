using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNet.DependencyInjection;
using EasyNet.Domain.Entities;
using EasyNet.Domain.Services;
using EasyNet.Dto;

namespace EasyNet.ApplicationService
{
    public abstract class EasyNetQueryWithDomainServerAppService<TEntity, TEntityDto, TDomainService, TGetAllInput> : EasyNetQueryWithDomainServerAppService<TEntity, TEntityDto, int, TDomainService, TGetAllInput>
        where TEntity : class, IEntity<int>
        where TEntityDto : IEntityDto<int>
        where TDomainService : EasyNetQueryDomainService<TEntity, int>
    {
        protected EasyNetQueryWithDomainServerAppService(IIocResolver iocResolver, TDomainService domainService) : base(iocResolver, domainService)
        {
        }
    }

    /// <summary>
    /// Derive your application services from this class.
    /// </summary>

    public abstract class EasyNetQueryWithDomainServerAppService<TEntity, TEntityDto, TPrimaryKey, TDomainService, TGetAllInput> : EasyNetAppService
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TDomainService : EasyNetQueryDomainService<TEntity, TPrimaryKey>
    {
        protected EasyNetQueryWithDomainServerAppService(IIocResolver iocResolver, TDomainService domainService) : base(iocResolver)
        {
            DomainService = domainService;
        }

        protected TDomainService DomainService { get; }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TEntityDto> GetAsync(TPrimaryKey id)
        {
            var entity = await DomainService.GetByIdAsync(id);

            return MapToEntityDto(entity);
        }

        /// <summary>
        /// Get all
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<TEntityDto>> GetAllAsync(TGetAllInput input)
        {
            var entities = await DomainService.GetAllAsync();

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
