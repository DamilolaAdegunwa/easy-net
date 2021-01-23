using System.Threading.Tasks;
using EasyNet.DependencyInjection;
using EasyNet.Domain.Entities;
using EasyNet.Domain.Repositories;
using EasyNet.Dto;

namespace EasyNet.ApplicationService
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class EasyNetCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput> : EasyNetCrudAppService<TEntity,
            TEntityDto, TPrimaryKey, TGetAllInput, TEntityDto, TEntityDto>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected EasyNetCrudAppService(IIocResolver iocResolver, IRepository<TEntity, TPrimaryKey> repository) : base(iocResolver, repository)
        {
        }
    }

    public abstract class EasyNetCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput> : EasyNetCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TEntityDto>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected EasyNetCrudAppService(IIocResolver iocResolver, IRepository<TEntity, TPrimaryKey> repository) : base(iocResolver, repository)
        {
        }
    }

    public abstract class EasyNetCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput> : EasyNetQueryAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput>, IEasyNetCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        protected EasyNetCrudAppService(IIocResolver iocResolver, IRepository<TEntity, TPrimaryKey> repository) : base(iocResolver, repository)
        {
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TEntityDto> CreateAsync(TCreateInput input)
        {
            var entity = MapCreateInputToEntity(input);

            await Repository.InsertAndGetIdAsync(entity);

            return MapToEntityDto(entity);
        }

        protected virtual TEntity MapCreateInputToEntity(TCreateInput input)
        {
            var entity = ObjectMapper.Map<TEntity>(input);

            //if (entity is IMustHaveTenant mustHaveTenant)
            //{
            //    mustHaveTenant.TenantId = GetUsingTenantId();
            //}
            //else if (entity is IMayHaveTenant mayHaveTenant)
            //{
            //    mayHaveTenant.TenantId = UsingTenantId;
            //}

            if (entity is IPassivable passivable)
            {
                passivable.IsActive = true;
            }

            return entity;
        }

        /// <inheritdoc/>
        public virtual async Task<TEntityDto> UpdateAsync(TUpdateInput input)
        {
            var entity = await Repository.GetAsync(input.Id);

            MapUpdateInputToEntity(input, entity);

            await Repository.UpdateAsync(entity);

            return MapToEntityDto(entity);
        }

        protected virtual void MapUpdateInputToEntity(TUpdateInput input, TEntity entity)
        {
            ObjectMapper.Map(input, entity);
        }

        /// <inheritdoc/>
        public virtual Task DeleteAsync(TPrimaryKey id)
        {
            return Repository.DeleteAsync(id);
        }
    }
}
