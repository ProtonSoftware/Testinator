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

        /// <summary>
        /// Indicates if next question after adding previous one in TestEditor
        /// should be the same Type as it was in this previous one
        /// If false - then next question is blank page and user can choose which Type he wants now
        /// </summary>
        public bool IsNextQuestionTypeTheSame { get; set; } = true; // Set this in UI, for now - true 

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
