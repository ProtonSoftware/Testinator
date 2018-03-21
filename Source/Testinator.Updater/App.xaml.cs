using System.Globalization;
using System.Windows;
using Testinator.Core;

namespace Testinator.Updater
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Do everything that base application needs to do
            base.OnStartup(e);

            // Get application type from first argument (Client or Server)
            var applicationType = e.Args.Length > 0 ? e.Args[0] : string.Empty;

            // Based on that...
            switch (applicationType)
            {
                case "Client":
                    UpdaterSettings.AppType = ApplicationType.Client;
                    break;
                case "Server":
                    UpdaterSettings.AppType = ApplicationType.Server;
                    break;
                default:
                    // Application was run incorrectly, abort it
                    Current.Shutdown();
                    return;
            }

            // Set application language
            LocalizationResource.Culture = new CultureInfo(e.Args[1]);

            // Setup IoC
            IoCUpdater.Setup();

            // Show main application window
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
        }
    }
}
