using System.Globalization;
using System.Windows;
using Testinator.Core;
using Testinator.Server.Core;
using Testinator.UICore;

namespace Testinator.Server
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

            // Check for updates
            if (CheckUpdates())
            {
                // Run the updater

                // Close this app
            }

            // Setup the main application 
            ApplicationSetup();

            // Log that application is starting
            IoCServer.Logger.Log("Application starting...");

            // Show the main window
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
        }

        /// <summary>
        /// Configures our application ready for use
        /// </summary>
        private void ApplicationSetup()
        {
            // Setup IoC
            IoCServer.Setup();

            // Bind a logger
            IoCServer.Kernel.Bind<ILogFactory>().ToConstant(new BaseLogFactory(new[]
            {
                // Set the path from Settings
                new FileLogger(IoCServer.Settings.LogFilePath),
            }));

            // Bind a File Writer
            IoCServer.Kernel.Bind<FileManagerBase>().ToConstant(new LogsWriter());

            // Bind a UI Manager
            IoCServer.Kernel.Bind<IUIManager>().ToConstant(new UIManager());

            // Set application language
            // TODO: Set this remotely, not only at the start
            LocalizationResource.Culture = new CultureInfo(IoCServer.Application.ApplicationLanguage);
        }

        /// <summary>
        /// Checks if there is a new version of that application
        /// </summary>
        private bool CheckUpdates()
        {
            // Get the newest version
            try
            {

            }
            catch
            {
                // Cannot connect to the web, no updates
                return false;
            }

            // No newer version available
            return false;
        }
    }
}
