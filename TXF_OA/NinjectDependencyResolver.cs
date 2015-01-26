using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using Ninject.Modules;
using TXF.IOC;

namespace TXF_OA
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver()
        {
            this.kernel = new StandardKernel();
            RegisterServices(kernel);
        }
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Load(new List<INinjectModule> { new BLLModule(), new DaoModule() });
        }   
        public object GetService(Type serviceType)
        {
            return this.kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.kernel.GetAll(serviceType);
        }
    }
}
