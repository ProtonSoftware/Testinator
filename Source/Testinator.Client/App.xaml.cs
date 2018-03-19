using System;
using System.Globalization;
using System.Windows;
using Testinator.Client.Core;
using Testinator.Core;
using Testinator.UICore;

namespace Testinator.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Custom startup so we load our IoC immediately before anything else
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            // Let the base application do what it needs
            base.OnStartup(e);

            // Setup the main application 
            ApplicationSetup();

            // Log that application is starting
            IoCClient.Logger.Log("Application starting...");

            // Show the main window
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
        }

        /// <summary>
        /// Configures our application ready for use
        /// </summary>
        private void ApplicationSetup()
        {
            // Set default language
            LocalizationResource.Culture = new CultureInfo("pl-PL");

            // Setup IoC
            IoCClient.Setup();

            // Bind a logger
            IoCClient.Kernel.Bind<ILogFactory>().ToConstant(new BaseLogFactory(new[]
            {
                // TODO: Add ApplicationSettings so we can set/edit a log location
                //       For now just log to the path where this application is running

                // TODO: remove this random numbe, but for now I cannot run multiple instances to test them coz this file is used by all of them
                //       and causes a crash
                new FileLogger(($"log{new Random().Next(100000, 99999999).ToString()}.txt")),
            }));

            // Bind a File Writer
            IoCClient.Kernel.Bind<FileManagerBase>().ToConstant(new LogsWriter());

            // Bind a UI Manager
            IoCClient.Kernel.Bind<IUIManager>().ToConstant(new UIManager());
        }
    }
}
