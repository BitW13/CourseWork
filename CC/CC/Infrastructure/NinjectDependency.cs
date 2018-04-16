using CC.Context;
using CC.Models.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CC.Models.Concrete;
using CC.Context.ContextModels;

namespace CC.Infrastructure
{
    public class NinjectDependency : IDependencyResolver
    {
        private IKernel _kernel;

        public NinjectDependency(IKernel kernel)
        {
            _kernel = kernel;
            AddBinding();
        }

        private void AddBinding()
        {
            _kernel.Bind<IRepository<User>>().To<EFUserRepository>();
            _kernel.Bind<IRepository<Cafe>>().To<EFCafeRepository>();
            _kernel.Bind<IRepository<Record>>().To<EFRecordRepository>();
            _kernel.Bind<IRepository<Host>>().To<EFHostRepostitory>();
            //_kernel.Bind<IRepository<Chat>>().To<EFChatRepository>();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }
    }
}