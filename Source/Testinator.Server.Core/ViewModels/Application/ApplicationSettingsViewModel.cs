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

        /// <summary>
        /// The path to the log file of this application
        /// </summary>
        public string LogFilePath { get; set; } = "log.txt";

        /// <summary>
        /// If false, prevent showing informational message boxes in this application
        /// </summary>
        public bool AreInformationMessageBoxesAllowed { get; set; } = true;

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
            // Load every property data from config file
            ReadDataFromConfig();

            // Hook to property changed event, so everytime settings are being changed, we save it to the file
            PropertyChanged += SaveSettingsStateToFile;
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Loads every property from config file to this view model at the start
        /// </summary>
        private void ReadDataFromConfig()
        {
            // TODO: Config file XmlReader
        }

        /// <summary>
        /// Saves current application settings state to the configuration file
        /// </summary>
        private void SaveSettingsStateToFile(object sender, PropertyChangedEventArgs e)
        {
            // TODO: Collect data and send it to the XmlWriter

            // Jest zrobiony writetofile w xmlwriterze, tylko trzeba readera zrobic aby to w pelni dzialalo
        }

        #endregion
    }
}
