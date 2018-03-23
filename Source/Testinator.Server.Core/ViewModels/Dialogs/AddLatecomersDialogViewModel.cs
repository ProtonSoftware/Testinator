using System.Collections.Generic;
using System.Collections.ObjectModel;
using Testinator.Core;

namespace Testinator.Server.Core
{
    public class ListViewModel<T> : BaseViewModel
        where T : class
    {
        public ObservableCollection<ListItemViewModel<T>> Items { get; set; } = new ObservableCollection<ListItemViewModel<T>>();

        public ListViewModel(List<T> Data)
        {
            foreach (var item in Data)
                Items.Add(new ListItemViewModel<T>(item));
        }

        public List<T> GetSelectedItems()
        {
            var result = new List<T>();

            foreach (var item in Items)
                if (item.IsSelected)
                    result.Add(item.Object);

            return result;
        }

        public void SelectAll()
        {
            foreach (var item in Items)
                item.IsSelected = true;
        }

        public void SelectNone()
        {
            foreach (var item in Items)
                item.IsSelected = false;
        }
    }

    public class ListItemViewModel<T> : BaseViewModel
    where T : class
    {
        public T Object { get; set; }
        public bool IsSelected { get; set; }

        public ListItemViewModel(T Item)
        {
            Object = Item;
        }
    }

    /// <summary>
    /// A viewmodel for dialog box to show possible users that can be added to the current test sessions,
    /// and ask the user to determine who they want to add to the test
    /// </summary>
    public class AddLatecomersDialogViewModel: BaseDialogViewModel
    {
        #region Private Members

        /// <summary>
        /// Indicates if select all checkbox is checked or not
        /// </summary>
        private bool mAllSelected;

        #endregion

        #region Public Properties

        /// <summary>
        /// The message to display
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The text to use for the OK button
        /// </summary>
        public string AcceptText { get; set; } = "OK";

        /// <summary>
        /// The text to use for the Cancel button
        /// </summary>
        public string CancelText { get; set; } = "Cancel";

        /// <summary>
        /// Indicates if select all checkbox is checked or not
        /// </summary>
        public bool AllSelected
        {
            get => mAllSelected;
            set
            {
                mAllSelected = value;
                SelectionChanged(value);
            }
        }

        /// <summary>
        /// The clients user agreed to add
        /// </summary>
        public List<ClientModel> UserResponse { get; set; } = new List<ClientModel>();

        /// <summary>
        /// The viewmodel for the data grid
        /// </summary>
        public ListViewModel<ClientModel> ListViewModel { get; set; }

        #endregion

        #region Constructor
        
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="CanStartTestClients">The base of this viewmodel which is the list of all clients that can possible start a test</param>
        public AddLatecomersDialogViewModel(List<ClientModel>CanStartTestClients)
        {
            ListViewModel = new ListViewModel<ClientModel>(CanStartTestClients);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Fired when <see cref="AllSelected"/> flag changes state
        /// </summary>
        public void SelectionChanged(bool selectAll)
        {
            if (selectAll)
                ListViewModel.SelectAll();
            else
                ListViewModel.SelectNone();
        }

        /// <summary>
        /// Closes message box and saves results
        /// </summary>
        public void AcceptAndClose()
        {
            UserResponse = ListViewModel.GetSelectedItems();
        }

        /// <summary>
        /// Closes message box without saving results
        /// </summary>
        public void CancelAndClose()
        {
        }

        #endregion
    }
}
