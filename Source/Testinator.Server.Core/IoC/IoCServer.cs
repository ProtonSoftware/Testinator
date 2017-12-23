using Ninject;
using Testinator.Network.Server;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The IoC container for the server application
    /// </summary>
    public static class IoCServer
    {
        #region Public Properties

        /// <summary>
        /// The kernel for our IoC container
        /// </summary>
        public static IKernel Kernel { get; private set; } = new StandardKernel();

        /// <summary>
        /// A shortcut to access the <see cref="ApplicationViewModel"/>
        /// </summary>
        public static ApplicationViewModel Application => IoCServer.Get<ApplicationViewModel>();

        /// <summary>
        /// A shortcut to access the <see cref="ServerNetwork"/>
        /// A sho/// </summary>
        public static ServerNetwork Network => IoCServer.Get<ServerNetwork>();

        /// <summary>
        /// A shortcut to access the <see cref="IUIManager"/>
        /// </summary>
        public static IUIManager UI => IoCServer.Get<IUIManager>();

        #endregion

        #region Construction

        /// <summary>
        /// Sets up the IoC container, binds all information required and is ready for use
        /// NOTE: Must be called as soon as your application starts up to ensure all 
        ///       services can be found
        /// </summary>
        public static void Setup()
        {
            // Bind all required view models
            BindViewModels();
        }

        /// <summary>
        /// Binds all singleton view models
        /// </summary>
        private static void BindViewModels()
        {
            // Bind to a single instance of Application view model
            Kernel.Bind<ApplicationViewModel>().ToConstant(new ApplicationViewModel());
            Kernel.Bind<ServerNetwork>().ToConstant(new ServerNetwork());
        }

        #endregion

        /// <summary>
        /// Get's a service from the IoC, of the specified type
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            return Kernel.Get<T>();
        }
    }
}
