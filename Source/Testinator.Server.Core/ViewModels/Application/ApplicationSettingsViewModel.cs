using System.ComponentModel;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The view model for global server application settings
    /// </summary>
    public class ApplicationSettingsViewModel : BaseViewModel
    {
        #region Public Properties
        
        // Tutaj ustawienia jakie maja byc na page'u

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApplicationSettingsViewModel()
        {
            // Hook to property changed event, so everytime settings are being changed, we save it to the file
            PropertyChanged += SaveSettingsStateToFile;
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Saves current application settings state to the configuration file
        /// </summary>
        private void SaveSettingsStateToFile(object sender, PropertyChangedEventArgs e)
        {
            // TODO: Collect data and send it to the XmlWriter

            // Jak cos sie zmienia w ustawieniach to tutaj sie odpala ta funkcja
            // Trzeba zrobic wysylanie do xmlwritera i do pliku z configuracja wyslac zmiane

            // Przydaloby sie jeszcze wczytanie z configuracji na starcie applikacji zrobic
            // Np. w app.xaml.cs onstartup dac IoCServer.Settings.ReadDataFromConfig() czy cos
        }

        #endregion
    }
}
