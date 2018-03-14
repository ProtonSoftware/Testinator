using System;
using System.ComponentModel;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The view model for global server application settings
    /// </summary>
    public class ApplicationSettingsViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// Index of the language used in this application
        /// 0 - Polish
        /// 1 - English
        /// </summary>
        private int mLanguageIndex = 0;

        #endregion

        #region Private Properties

        /// <summary>
        /// Allows to get the property of this view model by simply calling its name
        /// </summary>
        /// <param name="propertyName">The name of the property to get/set</param>
        private object this[string propertyName]
        {
            get => GetType().GetProperty(propertyName).GetValue(this, null);
            set => GetType().GetProperty(propertyName).SetValue(this, value, null);
        }

        /// <summary>
        /// The config xml file reader which handles config properties loading from local config file
        /// </summary>
        private XmlReader ConfigFileReader { get; set; } = new XmlReader(SaveableObjects.Config);

        /// <summary>
        /// The config xml file writer which handles config properties saving/deleting from local config file
        /// </summary>
        private XmlWriter ConfigFileWriter { get; set; } = new XmlWriter(SaveableObjects.Config);

        #endregion

        #region Public Properties // TODO: Set this in UI

        /// <summary>
        /// The path to the log file of this application
        /// </summary>
        public string LogFilePath { get; set; } = "log.txt";

        /// <summary>
        /// If false, prevent showing informational message boxes in this application
        /// </summary>
        public bool AreInformationMessageBoxesAllowed { get; set; } = true;

        /// <summary>
        /// Indicates if next question after adding previous one in TestEditor
        /// should be the same Type as it was in this previous one
        /// If false - then next question is blank page and user can choose which Type he wants now
        /// </summary>
        public bool IsNextQuestionTypeTheSame { get; set; } = true;

        /// <summary>
        /// Index of the language used in this application
        /// 0 - Polish
        /// 1 - English
        /// </summary>
        public int LanguageIndex
        {
            get => mLanguageIndex;
            set
            {
                // Set new value
                mLanguageIndex = value;

                // Change app's language based on that
                switch(mLanguageIndex)
                {
                    case 1:
                        IoCServer.UI.ChangeLanguage("en-US");
                        break;

                    // 0 or any not found index is default - Polish language
                    default:
                        IoCServer.UI.ChangeLanguage("pl-PL");
                        break;
                }
            }
        }

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
            try
            { 
                // Check if config file exists
                if (!ConfigFileReader.FileExists("config.xml"))
                    // If not, create brand-new one from current view model state
                    ConfigFileWriter.WriteToFile(this);

                // Get the list of every property saved in the config file
                var propertyList = ConfigFileReader.LoadConfig();

                // For each property...
                foreach (var property in propertyList)
                {
                    // Set saved value from config
                    this[property.Name] = property.Value;
                }
            }
            catch (Exception ex)
            {
                // If an error occured, show info to the user
                IoCServer.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Błąd wczytywania",
                    Message = "Nie udało się wczytać pliku konfiguracyjnego." +
                              "\nTreść błędu: " + ex.Message,
                    OkText = "Ok"
                });

                IoCServer.Logger.Log("Unable to read config from local folder, error message: " + ex.Message);
            }
        }

        /// <summary>
        /// Saves current application settings state to the configuration file
        /// </summary>
        private void SaveSettingsStateToFile(object sender, PropertyChangedEventArgs e)
        {
            // Catch the name of the property that changed
            var propertyName = e.PropertyName;

            // Replicate the property
            var property = new SettingsPropertyInfo
            {
                Name = propertyName,
                Type = this[propertyName].GetType(),
                Value = this[propertyName]
            };

            try
            { 
                // Send the property to the XmlWriter
                ConfigFileWriter.WriteToFile(property);
            }
            catch (Exception ex)
            {
                // If an error occured, show info to the user
                IoCServer.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Błąd zapisu",
                    Message = "Nie udało się zapisać nowej wartości do pliku konfiguracyjnego." +
                              "\nTreść błędu: " + ex.Message,
                    OkText = "Ok"
                });

                IoCServer.Logger.Log("Unable to write new property value to the config, error message: " + ex.Message);
            }
        }

        #endregion
    }
}
