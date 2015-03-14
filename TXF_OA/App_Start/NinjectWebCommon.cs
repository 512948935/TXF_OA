[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(TXF_OA.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(TXF_OA.App_Start.NinjectWebCommon), "Stop")]

namespace TXF_OA.App_Start
{
    using System;
    using System.Web;
    using System.Linq;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Extensions.Conventions;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind(scanner => scanner.From("IBLL","BLL")
               .Select(IsServiceType)
               .BindDefaultInterface()
               .Configure(binding => binding.InSingletonScope())
           );
            kernel.Bind(scanner => scanner.From("IDao", "Dao")
               .Select(IsServiceType)
               .BindDefaultInterface()
               .Configure(binding => binding.InSingletonScope())
           );         
        }
        private static bool IsServiceType(Type type)
        {
            return type.IsClass && type.GetInterfaces().Any(intface => intface.Name == "I" + type.Name);
        }
    }
}
