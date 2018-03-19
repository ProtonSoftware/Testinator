using System;
using System.Collections.Generic;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The view model for the results page
    /// </summary>
    public class TestResultsViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// View model for the list control
        /// </summary>
        public TestResultsListViewModel ListViewModel { get; private set; } = new TestResultsListViewModel();

        /// <summary>
        /// The list of all results found on the machine
        /// </summary>
        public List<TestResults> Results { get; private set; } = new List<TestResults>();

        /// <summary>
        /// The results binary file writer which handles results saving/deleting from local folder
        /// </summary>
        public BinaryWriter ResultFileWriter { get; private set; } = new BinaryWriter(SaveableObjects.Results);

        /// <summary>
        /// The date the test was taken (day)
        /// </summary>
        public string ResultsDateDay { get; private set; }

        /// <summary>
        /// The date the test was taken (hour)
        /// </summary>
        public string ResultsDateHour { get; private set; }

        /// <summary>
        /// The name of the test
        /// </summary>
        public string TestName { get; private set; }

        /// <summary>
        /// The number of people that took the test
        /// </summary>
        public string TestAttendeesNumber { get; private set; }

        /// <summary>
        /// Indicated if there is a test result currently selected
        /// </summary>
        public bool IsAnyItemSelected { get; private set; }

        /// <summary>
        /// The number of results loaded from the disk
        /// </summary>
        public int ItemsLoadedCount => ListViewModel.Items.Count;

        #endregion

        #region Commands

        /// <summary>
        /// Show details about the test
        /// </summary>
        public ICommand ShowDetailsCommnd { get; private set; }
        
        /// <summary>
        /// The command to delete currently selected results
        /// </summary>
        public ICommand DeleteCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestResultsViewModel()
        {
            // Create commands
            ShowDetailsCommnd = new RelayCommand(ShowDetails);
            DeleteCommand = new RelayCommand(DeleteResult);

            SetDefaults();

            ListViewModel.LoadItems();

            ListViewModel.ItemSelected += ListControl_ItemSelected;
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Show the result's details page
        /// </summary>
        private void ShowDetails()
        {
            var selectedItem = ListViewModel.SelectedItem();
            if (selectedItem == null)
                return;

            var viewmodel = new TestResultsDetailsViewModel(selectedItem);
            IoCServer.Application.GoToPage(ApplicationPage.TestResultsDetails, viewmodel);
        }

        /// <summary>
        /// Fired when an item is selected from the list
        /// </summary>
        /// <param name="item">Item that has been selected</param>
        private void ListControl_ItemSelected(TestResults item)
        {
            LoadViewModel(item);
            IsAnyItemSelected = true;
        }

        /// <summary>
        /// Deletes currently selected results 
        /// </summary>
        private void DeleteResult()
        {
            var selectedItem = ListViewModel.SelectedItem();
            if (selectedItem == null)
                return;

            var vm = new DecisionDialogViewModel()
            {
                Title = "Usuwanie rezultatu",
                Message = "Czy chcesz usunąć ten rezultat?",
                AcceptText = "Tak",
                CancelText = "Nie",
            };
            IoCServer.UI.ShowMessage(vm);

            if (vm.UserResponse == false)
                return;

            try
            {
                // Try to delete the file
                ResultFileWriter.DeleteFile(selectedItem);
            }
            catch (Exception ex)
            {
                // If an error occured, show info to the user
                IoCServer.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Błąd usuwania",
                    Message = "Nie udało się usunąć tego rezultatu." +
                              "\nTreść błędu: " + ex.Message,
                    OkText = "Ok"
                });

                IoCServer.Logger.Log("Unable to delete result from local folder, error message: " + ex.Message);
            }

            // Reload items
            ListViewModel.LoadItems();

            SetDefaults();
            OnPropertyChanged(nameof(ItemsLoadedCount));
        }
        
        #endregion

        #region Private Helpers

        /// <summary>
        /// Loads viewmodel properties with the given test resuts object
        /// </summary>
        /// <param name="value"></param>
        private void LoadViewModel(TestResults value)
        {
            if (value == null)
                return;

            ResultsDateDay = value.Date.ToShortDateString();
            ResultsDateHour = value.Date.ToShortTimeString();
            TestName = value.Test.Name;
            TestAttendeesNumber = value.Results.Count.ToString(); ;
        }

        /// <summary>
        /// Sets default values to the viewmodel properties
        /// </summary>
        private void SetDefaults()
        {
            ResultsDateDay = "-";
            ResultsDateHour = "-";
            TestName = "-";
            TestAttendeesNumber = "-";
            IsAnyItemSelected = false;
        }

        #endregion
    }
}
