using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using Testinator.Server.Core;
using Testinator.Core;
using Testinator.UICore;
using System.Net;

namespace Testinator.Server
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Indicates if we have important update to install
        /// </summary>
        private bool ImportantUpdate { get; set; }

        /// <summary>
        /// Custom startup so we load our IoC and Updater immediately before anything else
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnStartup(StartupEventArgs e)
        {
            // Let the base application do what it needs
            base.OnStartup(e);

            // Check for updates
            if (await CheckUpdatesAsync())
            {
                // Run the updater
                var a = 1;

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
                new FileLogger(IoCServer.Settings.LogFilePath)
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
        private async Task<bool> CheckUpdatesAsync()
        {
            try
            {
                // Get current version
                var currentVersion = "1.0.0.0";

                // Set webservice's url and parameters we want to send
                var url = "http://minorsonek.pl/testinator/data/index.php";
                var parameters = $"version={ currentVersion }&type=Server";

                // Catch the result
                var result = string.Empty;

                // Send request to webservice
                using (var wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    result = wc.UploadString(url, parameters);
                }

                // Return the statement based on result...
                switch (result)
                {
                    case "New update":
                        // There is new update, but not important one
                        return true;

                    case "New update IMP":
                        {
                            // An important update
                            ImportantUpdate = true;
                            return true;
                        }

                    default:
                        // No updates
                        return false;
                }
            }
            catch
            {
                // Cannot connect to the web, no updates
                return false;
            }
        }
    }
}
