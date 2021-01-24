using AutoMapper;
using EasyNet.DependencyInjection;
using EasyNet.Domain.Entities;
using EasyNet.Domain.Repositories;
using EasyNet.Runtime.Session;

namespace EasyNet.ApplicationService
{
    public abstract class EasyNetAppService
    {

        protected EasyNetAppService(IIocResolver iocResolver)
        {
            IocResolver = iocResolver;
        }

        protected IIocResolver IocResolver { get; }

        protected IEasyNetSession EasyNetSession => _asyNetSession ?? (_asyNetSession = IocResolver.GetService<IEasyNetSession>());
        private IEasyNetSession _asyNetSession;

        protected IMapper ObjectMapper => _objectMapper ?? (_objectMapper = IocResolver.GetService<IMapper>());
        private IMapper _objectMapper;
    }

    public abstract class EasyNetAppService<TEntity> : EasyNetAppService<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected EasyNetAppService(IIocResolver iocResolver, IRepository<TEntity, int> repository) : base(iocResolver, repository)
        {
        }
    }

    public abstract class EasyNetAppService<TEntity, TPrimaryKey> : EasyNetAppService
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected EasyNetAppService(IIocResolver iocResolver, IRepository<TEntity, TPrimaryKey> repository) : base(iocResolver)
        {
            Repository = repository;
        }

        protected IRepository<TEntity, TPrimaryKey> Repository { get; }
    }
}
