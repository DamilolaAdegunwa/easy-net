using AutoMapper;
using EasyNet.DependencyInjection;
using EasyNet.Domain.Entities;
using EasyNet.Domain.Repositories;
using EasyNet.Runtime.Session;

namespace EasyNet.Domain.Services
{
    public abstract class EasyNetDomainService
    {
        protected EasyNetDomainService(IIocResolver iocResolver)
        {
            IocResolver = iocResolver;
        }

        protected IIocResolver IocResolver { get; }

        protected IEasyNetSession EasyNetSession => _asyNetSession ?? (_asyNetSession = IocResolver.GetService<IEasyNetSession>());
        private IEasyNetSession _asyNetSession;

        protected IMapper ObjectMapper => _objectMapper ?? (_objectMapper = IocResolver.GetService<IMapper>());
        private IMapper _objectMapper;
    }

    public abstract class EasyNetDomainService<TEntity> : EasyNetDomainService<TEntity, int>
        where TEntity : class, IEntity<int>
    {

        protected EasyNetDomainService(IIocResolver iocResolver, IRepository<TEntity, int> repository) : base(iocResolver, repository)
        {
        }
    }

    public abstract class EasyNetDomainService<TEntity, TPrimaryKey> : EasyNetDomainService
        where TEntity : class, IEntity<TPrimaryKey>
    {


        protected EasyNetDomainService(IIocResolver iocResolver, IRepository<TEntity, TPrimaryKey> repository) : base(iocResolver)
        {
            Repository = repository;
        }

        protected IRepository<TEntity, TPrimaryKey> Repository { get; }
    }
}