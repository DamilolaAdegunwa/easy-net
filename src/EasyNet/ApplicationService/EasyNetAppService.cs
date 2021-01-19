using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EasyNet.DependencyInjection;

namespace EasyNet.ApplicationService
{
    public class EasyNetAppService : IEasyNetAppService
    {

        public EasyNetAppService(IIocResolver iocResolver)
        {
            IocResolver = iocResolver;
        }

        protected IIocResolver IocResolver { get; }

        protected IMapper ObjectMapper => _objectMapper ?? (_objectMapper = IocResolver.GetService<IMapper>());
        private IMapper _objectMapper = null;
    }
}
