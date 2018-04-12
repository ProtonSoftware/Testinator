using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// Viewmodel for images editor
    /// </summary>
    public class ImagesEditorViewModel : BaseViewModel
    {
        #region Singletone

        /// <summary>
        /// Single instance of this viewmodel
        /// </summary>
        public static ImagesEditorViewModel Instance { get; private set; } = new ImagesEditorViewModel();
        // Keep private setter as we want only one instance of this viewmodel to be availavle throughout the application

        #endregion

        #region Public Properties

        /// <summary>
        /// The items in this editor
        /// </summary>
        public ObservableCollection<ImagesEditorItemViewModel> Items { get; private set; } = new ObservableCollection<ImagesEditorItemViewModel>();

        /// <summary>
        /// All the images this editor holds
        /// </summary>
        public List<Bitmap> Images { get; private set; } = new List<Bitmap>();

        /// <summary>
        /// Indicates if the user can add more question to the task
        /// </summary>
        public bool CanAddImages => Items.Count < TaskContent.MaximumImagesCount;

        /// <summary>
        /// Indicates if previewmode is enabled
        /// </summary>
        public bool IsPreviewModeEnabled { get; private set; }

        /// <summary>
        /// Current image showed in preview
        /// </summary>
        public Image CurrentPreviewItem => Images.Count != 0 ? Images[CurrentPreviewItemIndex] : null;

        /// <summary>
        /// Index of the item being currently showed in preview
        /// </summary>
        public int CurrentPreviewItemIndex { get; private set; }

        /// <summary>
        /// Indicates if the preview tool can go forward
        /// </summary>
        public bool CanGoForward => CurrentPreviewItemIndex + 1 < Items.Count;

        /// <summary>
        /// Indicates if the preview tool can go back
        /// </summary>
        public bool CanGoBack => CurrentPreviewItemIndex > 0;
        
        #endregion

        #region Public Commands

        /// <summary>
        /// Adds an image if possible
        /// </summary>
        public ICommand AddImageCommand { get; private set; }

        /// <summary>
        /// The command to select an item for preview; opens the preview menu too
        /// </summary>
        public ICommand SelectItemForPreviewCommand { get; private set; }

        /// <summary>
        /// The command to delete an image
        /// </summary>
        public ICommand DeleteImageCommand { get; private set; }

        /// <summary>
        /// The command to close preview
        /// </summary>
        public ICommand ClosePreviewCommand { get; private set; }

        /// <summary>
        /// The command to go the next item in preview mode
        /// </summary>
        public ICommand GoNextPreviewItemCommand { get; private set; }

        /// <summary>
        /// The command to go the previous item in preview mode
        /// </summary>
        public ICommand GoPreviousPreviewItemCommand { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads items for to view
        /// </summary>
        /// <param name="newItems">Pass null to clear</param>
        public void LoadItems(List<Bitmap> newItems)
        {
            CurrentPreviewItemIndex = 0;
            IsPreviewModeEnabled = false;

            if(newItems == null)
            {
                Items.Clear();
                Images.Clear();
            }
            else
            {
                Images = newItems.ToList();
                Items.Clear();
                for (var i = 0; i < newItems.Count; i++)
                {
                    Items.Add(new ImagesEditorItemViewModel()
                    {
                        ID = i,
                        Thumbnail = newItems[i].GetThumbnail(),
                        OriginalImage = newItems[i],
                    });
                }
            }

            OnPropertyChanged(nameof(CanAddImages));
            OnPropertyChanged(nameof(CanGoForward));
            OnPropertyChanged(nameof(CanGoBack));
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Fired when an image is deleted
        /// </summary>
        public event Action<Bitmap> ImageDeleted = (e) => { };

        /// <summary>
        /// Fired when an image is added
        /// </summary>
        public event Action<Bitmap> ImageAdded = (e) => { };

        #endregion

        #region Command Methods

        /// <summary>
        /// Asks the user to choose an image from disk and adds it to the list
        /// </summary>
        private void AddImage()
        {
            if (CanAddImages)
            {
                // Move file types somewhere else
                var fileName = IoCServer.UI.ShowSingleFileDialog(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "png files (*.png)|*.png|jpg files (*.jpg)|*.jpg|bmp files (*.bmp)|*.bm/p");

                // The user did not selected any files
                if (string.IsNullOrEmpty(fileName))
                    return;

                Bitmap image;
                try
                {
                    image = new Bitmap(fileName);

                    TaskContent.ValidateImage(image);
                }
                catch (FileNotFoundException ex)
                {
                    IoCServer.UI.ShowMessage(new MessageBoxDialogViewModel()
                    {
                        Message = $"Nie można wczytać pliku. Błąd: {ex.Message}",
                        OkText = "OK",
                        Title = "Nie można dodać obrazka",
                    });
                    return;
                }
                catch (Exception ex)
                {
                    IoCServer.UI.ShowMessage(new MessageBoxDialogViewModel()
                    {
                        Message = ex.Message,
                        OkText = "OK",
                        Title = "Nie można dodać obrazka",
                    });
                    return;
                }

                Items.Add(new ImagesEditorItemViewModel()
                {
                    ID = Items.Count,
                    Thumbnail = image.GetThumbnail(),
                    OriginalImage = image,
                });

                Images.Add(image);

                ImageAdded.Invoke(image);
            }

            OnPropertyChanged(nameof(CanAddImages));
            OnPropertyChanged(nameof(CanGoForward));
            OnPropertyChanged(nameof(CanGoBack));
        }

        /// <summary>
        /// Show the preview menu starting from this item
        /// </summary>
        /// <param name="obj"></param>
        private void SelectItemForPreview(object obj)
        {
            var index = GetIndex(obj);

            if (index == -1)
                return;

            IsPreviewModeEnabled = true;
            CurrentPreviewItemIndex = index;
            OnPropertyChanged(nameof(CurrentPreviewItem));
        }

        /// <summary>
        /// Deletes an immage from the list
        /// </summary>
        /// <param name="obj"></param>
        private void DeleteImage(object obj)
        {
            var index = GetIndex(obj);

            if (index == -1)
                return;

            var DeletedImage = Images[index];

            Items.RemoveAt(index);
            Images.RemoveAt(index);

            UpdateIndexes(index);

            ImageDeleted.Invoke(DeletedImage);

            OnPropertyChanged(nameof(CanAddImages));
            OnPropertyChanged(nameof(CanGoForward));
            OnPropertyChanged(nameof(CanGoBack));
        }

        /// <summary>
        /// Goes to the next preview item
        /// </summary>
        private void GoPreviousPreviewItem()
        {
            if (CanGoBack)
                CurrentPreviewItemIndex--;

            OnPropertyChanged(nameof(CurrentPreviewItem));
        }

        /// <summary>
        /// Goes to the previous preview item
        /// </summary>
        private void GoNextPreviewItem()
        {
            if (CanGoForward)
                CurrentPreviewItemIndex++;

            OnPropertyChanged(nameof(CurrentPreviewItem));
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ImagesEditorViewModel()
        {
            CreateCommands();
            LoadItems(null);
        }

        /// <summary>
        /// Constructor with initialization data
        /// </summary>
        /// <param name="model">The intial data for this viewmodel</param>
        public ImagesEditorViewModel(List<Bitmap> model)
        {
            CreateCommands();
            LoadItems(model);
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Creates commands for this viewmodel
        /// </summary>
        private void CreateCommands()
        {
            DeleteImageCommand = new RelayParameterizedCommand(DeleteImage);
            SelectItemForPreviewCommand = new RelayParameterizedCommand(SelectItemForPreview);
            ClosePreviewCommand = new RelayCommand(() => IsPreviewModeEnabled = false);
            GoNextPreviewItemCommand = new RelayCommand(GoNextPreviewItem);
            GoPreviousPreviewItemCommand = new RelayCommand(GoPreviousPreviewItem);
            AddImageCommand = new RelayCommand(AddImage);
        }

        /// <summary>
        /// Gets the index from a command parameter
        /// </summary>
        /// <param name="param"></param>
        /// <returns>-1 if invalid</returns>
        private int GetIndex(object param)
        {
            int index;
            try
            {
                index = (int)param;

                if (index >= Items.Count)
                    throw new Exception();
            }
            catch
            {
                IoCServer.Logger.Log("Developer error. Code: 2");
                return -1;
            }
            return index;
        }

        /// <summary>
        /// Updates indexes of items collection from the given index
        /// </summary>
        /// <param name="strat"></param>
        private void UpdateIndexes(int start)
        {
            for (var i = start > 0 ? start - 1 : 0; i < Items.Count; i++)
                Items[i].ID = i;
        }

        #endregion
    }
}
