using System.Threading.Tasks;
using EasyNet.DependencyInjection;
using EasyNet.Domain.Entities;
using EasyNet.Domain.Repositories;
using EasyNet.Dto;

namespace EasyNet.ApplicationService
{
    public abstract class EasyNetCrudAppService<TEntity, TEntityDto, TGetAllInput> : EasyNetCrudAppService<TEntity, TEntityDto, int, TGetAllInput, TEntityDto, TEntityDto>, IEasyNetCrudAppService<TEntityDto>
        where TEntity : class, IEntity<int>
        where TEntityDto : IEntityDto<int>
    {
        protected EasyNetCrudAppService(IIocResolver iocResolver, IRepository<TEntity, int> repository) : base(iocResolver, repository)
        {
        }
    }

    public abstract class EasyNetCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput> : EasyNetCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TEntityDto, TEntityDto>, IEasyNetCrudAppService<TEntityDto, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected EasyNetCrudAppService(IIocResolver iocResolver, IRepository<TEntity, TPrimaryKey> repository) : base(iocResolver, repository)
        {
        }
    }

    public abstract class EasyNetCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput> : EasyNetCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TEntityDto>, IEasyNetCrudAppService<TEntityDto, TPrimaryKey, TCreateInput>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected EasyNetCrudAppService(IIocResolver iocResolver, IRepository<TEntity, TPrimaryKey> repository) : base(iocResolver, repository)
        {
        }
    }

    public abstract class EasyNetCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput> : EasyNetQueryAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput>, IEasyNetCrudAppService<TEntityDto, TPrimaryKey, TCreateInput, TUpdateInput>
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

        /// <summary>
        /// Map create input to entity
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TEntityDto> UpdateAsync(TUpdateInput input)
        {
            var entity = await Repository.GetAsync(input.Id);

            MapUpdateInputToEntity(input, entity);

            await Repository.UpdateAsync(entity);

            return MapToEntityDto(entity);
        }

        /// <summary>
        /// Map update input to entity
        /// </summary>
        /// <param name="input"></param>
        /// <param name="entity"></param>
        protected virtual void MapUpdateInputToEntity(TUpdateInput input, TEntity entity)
        {
            ObjectMapper.Map(input, entity);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task DeleteAsync(TPrimaryKey id)
        {
            return Repository.DeleteAsync(id);
        }
    }
}
