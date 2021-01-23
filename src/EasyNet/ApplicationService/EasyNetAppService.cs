using AutoMapper;
using EasyNet.DependencyInjection;
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
}
