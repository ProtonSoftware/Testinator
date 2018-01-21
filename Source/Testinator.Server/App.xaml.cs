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
            // Set default language
            LocalizationResource.Culture = new CultureInfo("pl-PL");

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
        }
    }
}
