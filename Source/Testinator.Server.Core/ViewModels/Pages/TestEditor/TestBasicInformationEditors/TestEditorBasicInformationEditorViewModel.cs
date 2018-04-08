using System;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The view model for editing basic information about a test
    /// </summary>
    public class TestEditorBasicInformationEditorViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Original information about the test
        /// </summary>
        private TestInformation mOriginalInfo;

        #region Flags and others

        /// <summary>
        /// Indicates if this viewmodel is in editing mode
        /// If not creation mode is presumed
        /// </summary>
        public bool EditingModeOn { get; private set; }

        /// <summary>
        /// The error message that is currently showed
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Indicates if error message should be visible
        /// </summary>
        public bool ErrorMessageVisible => !string.IsNullOrEmpty(ErrorMessage);

        /// <summary>
        /// Maximum length of a test name
        /// </summary>
        public string MaxNameLength => TestBuilder.MaximumTestNameLength.ToString();

        /// <summary>
        /// Maximum length of a note
        /// </summary>
        public string MaxNoteLength => TestBuilder.MaximumNoteLength.ToString();

        #endregion

        #region Editable Fields

        /// <summary>
        /// The name of the test
        /// </summary>
        public string Name { get; set; } = "dddddddd";

        /// <summary>
        /// Duration in hours
        /// </summary>
        public string DurationHours { get; set; }

        /// <summary>
        /// Duration in minutes
        /// </summary>
        public string DurationMinutes { get; set; } = "22";

        /// <summary>
        /// Duration in seconds
        /// </summary>
        public string DurationSeconds { get; set; }

        /// <summary>
        /// Tags for this test
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// The note saved with this test
        /// </summary>
        public string Note { get; set; }

        #endregion

        #endregion

        #region Commands 

        /// <summary>
        /// The command to submit current page
        /// </summary>
        public ICommand SubmitCommand { get; private set; }

        /// <summary>
        /// The command to exit completely from this editor
        /// </summary>
        public ICommand ExitCommand { get; private set; }

        /// <summary>
        /// The command to come back to the previous page
        /// </summary>
        public ICommand ReturnCommand { get; private set; }

        #endregion

        #region Command Methods

        /// <summary>
        /// Submits current page
        /// </summary>
        private void Submit()
        {
            int durationHours = 0, durationMinutes = 0 , durationSeconds = 0;

            try
            {
                // Follow the order of properties like in the view

                IoCServer.TestEditor.Builder.AddName(Name);

                if (string.IsNullOrEmpty(DurationHours))
                    DurationHours = "00";

                if (string.IsNullOrEmpty(DurationMinutes))
                    DurationMinutes = "00";

                if (string.IsNullOrEmpty(DurationSeconds))
                    DurationSeconds = "00";

                if ((DurationHours.TrimStart('0') != "" && !int.TryParse(DurationHours.TrimStart('0'), out durationHours)) || durationHours > 3)
                    throw new Exception("Niepoprawna wartość w polu godzin.");

                if ((DurationMinutes.TrimStart('0') != "" && !int.TryParse(DurationMinutes.TrimStart('0'), out durationMinutes)) || durationMinutes > 60)
                    throw new Exception("Niepoprawna wartość w polu minut.");


                if ((DurationSeconds.TrimStart('0') != "" && !int.TryParse(DurationSeconds.TrimStart('0'), out durationSeconds)) || durationSeconds > 60)
                    throw new Exception("Niepoprawna wartość w polu sekund.");


                IoCServer.TestEditor.Builder.AddDuration(new TimeSpan(durationHours, durationMinutes, durationSeconds));

                IoCServer.TestEditor.Builder.AddTags(Tags);

                IoCServer.TestEditor.Builder.AddNote(Note);

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return;
            }

            // Chcek if something changed compared to the original information
            if (EditingModeOn)
            {
                if (mOriginalInfo.Name != Name || mOriginalInfo.Duration.TotalSeconds != (durationHours * 3600 + durationMinutes * 60 + durationSeconds) ||
                  mOriginalInfo.Tags != Tags || mOriginalInfo.Note != Note)
                    IoCServer.TestEditor.TestChanged();
            }

            // If everything went fine proceed to the next phase of creation/edition of this test
            IoCServer.TestEditor.GoNextPhase();
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates this viewmodel with edit mode off so it means that it is creating a new test not editing one
        /// </summary>
        public TestEditorBasicInformationEditorViewModel()
        {
            CreateCommands();
        }

        /// <summary>
        /// Create this viewmodel with edit mode on so it means that it is editing an existing test
        /// </summary>
        /// <param name="TestInfo"></param>
        public TestEditorBasicInformationEditorViewModel(TestInformation TestInfo)
        {
            CreateCommands();

            if (TestInfo == null)
                EditingModeOn = false;
            else
            {
                EditingModeOn = true;

                mOriginalInfo = TestInfo;

                // Fill initial values
                Name = TestInfo.Name;
                DurationHours = TestInfo.Duration.Hours.ToString();
                DurationMinutes = TestInfo.Duration.Minutes.ToString();
                DurationSeconds = TestInfo.Duration.Seconds.ToString();
                Tags = TestInfo.Tags;
                Note = TestInfo.Note;
            }
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Creates commands for this viewmodel
        /// </summary>
        private void CreateCommands()
        {
            SubmitCommand = new RelayCommand(Submit);
            ReturnCommand = new RelayCommand(() => IoCServer.TestEditor.Return());
            ExitCommand = new RelayCommand(() => IoCServer.TestEditor.Exit());
        }

        #endregion
    }
}
