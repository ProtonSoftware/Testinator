﻿using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Client.Core
{
    /// <summary>
    /// The view model for a text entry to edit a string value
    /// <summary>
    public class TextEntryViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The label to identify what this value is for
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The current saved value
        /// </summary>
        public string OriginalText { get; set; }

        /// <summary>
        /// The current non-commit edited text
        /// </summary>
        public string EditedText { get; set; }

        /// <summary>
        /// Indicates if the current text is in edit mode
        /// </summary>
        public bool Editing { get; set; } = false;

        #endregion

        #region Commands

        /// <summary>
        /// Puts the control into edit mode
        /// </summary>
        public ICommand EditCommand { get; set; }

        /// <summary>
        /// Cancels out of edit mode
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// Commits the edits and saves the value
        /// as well as goes back to non-edit mode
        /// </summary>
        public ICommand SaveCommand { get; set; }

        #endregion

        #region Constructor 

        /// <summary>
        /// Default constructor
        /// </summary>
        public TextEntryViewModel()
        {
            // Create commands
            EditCommand = new RelayCommand(Edit);
            CancelCommand = new RelayCommand(Cancel);
            SaveCommand = new RelayCommand(Save);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Puts the control into edit mode
        /// </summary>
        public void Edit()
        {
            // Set the edited text to the current value
            EditedText = OriginalText;

            // Go into edit mode
            Editing = true;
        }

        /// <summary>
        /// Cancels out of edit mode
        /// </summary>
        public void Cancel()
        {
            // Get out of editing mode
            Editing = false;
        }

        /// <summary>
        /// Commits the content and exits out of edit mode
        /// </summary>
        public void Save()
        {
            // Save content
            OriginalText = EditedText;
            switch (Label)
            {
                case "Imię": IoCClient.Client.ClientName = EditedText;
                    break;

                case "Nazwisko": IoCClient.Client.ClientSurname = EditedText;
                    break;
            }

            // Get out of editing mode
            Editing = false;
        }

        #endregion
    }
}
