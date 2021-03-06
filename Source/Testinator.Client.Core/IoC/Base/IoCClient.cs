﻿using Ninject;
using System;
using Testinator.Core;

namespace Testinator.Client.Core
{
    /// <summary>
    /// The IoC container for the client application
    /// </summary>
    public static class IoCClient
    {
        #region Public Properties

        /// <summary>
        /// The kernel for our IoC container
        /// </summary>
        public static IKernel Kernel { get; private set; } = new StandardKernel();

        /// <summary>
        /// A shortcut to access the <see cref="ApplicationViewModel"/>
        /// </summary>
        public static ApplicationViewModel Application => IoCClient.Get<ApplicationViewModel>();

        /// <summary>
        /// A shortcut to access the <see cref="ClientModel"/>
        /// </summary>
        public static ClientModel Client => IoCClient.Get<ClientModel>();

        /// <summary>
        /// A shortcut to access the <see cref="TestHost"/>
        /// </summary>
        public static TestHost TestHost => IoCClient.Get<TestHost>();

        /// <summary>
        /// A shortcut to access the <see cref="FileManagerBase"/>
        /// </summary>
        public static FileManagerBase File => IoCClient.Get<FileManagerBase>();

        /// <summary>
        /// A shortcut to access the <see cref="IUIManager"/>
        /// </summary>
        public static IUIManager UI => IoCClient.Get<IUIManager>();

        /// <summary>
        /// A shortcut to access the <see cref="ILogFactory"/>
        /// </summary>
        public static ILogFactory Logger => IoCClient.Get<ILogFactory>();

        #endregion

        #region Public Events

        /// <summary>
        /// Fired when IoC completes its setup
        /// </summary>
        public static event Action SetupCompleted = () => { };

        #endregion

        #region Construction

        /// <summary>
        /// Sets up the IoC container, binds all information required and is ready for use
        /// NOTE: Must be called as soon as our application starts up to ensure all 
        ///       services can be found
        /// </summary>
        public static void Setup()
        {
            // Bind all required view models
            BindViewModels();
            SetupCompleted.Invoke();
        }

        /// <summary>
        /// Binds all singleton view models
        /// </summary>
        private static void BindViewModels()
        {
            // Bind to a single instance of every model here
            Kernel.Bind<ApplicationViewModel>().ToConstant(new ApplicationViewModel());
            Kernel.Bind<ClientModel>().ToConstant(new ClientModel());
            Kernel.Bind<TestHost>().ToConstant(new TestHost());
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
