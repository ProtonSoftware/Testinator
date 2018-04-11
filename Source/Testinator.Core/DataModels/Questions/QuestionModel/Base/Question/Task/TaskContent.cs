using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Testinator.Core
{
    /// <summary>
    /// The content of any task
    /// </summary>
    [Serializable]
    public class TaskContent
    {
        #region Public Properties

        /// <summary>
        /// The string content of this task
        /// </summary>
        public string StringContent { get; private set; }

        /// <summary>
        /// All images attached to this task
        /// </summary>
        public List<Bitmap> Images { get; private set; }

        /// <summary>
        /// The maximum number of immages attached to the task
        /// </summary>
        public const int MaximumImagesCount = 3;

        /// <summary>
        /// Maximum height of an image
        /// </summary>
        public const int MaxiumumHeight = 1000;

        /// <summary>
        /// Maximum width of an image
        /// </summary>
        public const int MaxiumumWidth = 1000;

        #endregion

        #region Constructr

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="Content">The string content of this task; cannot be null or empty</param>
        public TaskContent(string Content)
        {
            AddStringContent(Content);

            // Create defaults
            Images = new List<Bitmap>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the string content of this task
        /// </summary>
        /// <param name="Content">New content for this task</param>
        public void AddStringContent(string Content)
        {
            if (string.IsNullOrEmpty(Content))
                throw new NullReferenceException("Treść pytania nie może być pusta!");

            StringContent = Content;
        }

        /// <summary>
        /// Checks if this task has any images attached
        /// </summary>
        /// <returns>True if there is at leat one immage; otherwise, false</returns>
        public bool HasImages => Images.Count != 0;

        /// <summary>
        /// Adds an image to the task
        /// </summary>
        /// <param name="Image">The image to be added</param>
        public void AddImage(Bitmap Image)
        {
            if (Images.Count >= MaximumImagesCount)
                throw new Exception($"Maximum number of images is: {MaximumImagesCount}. Cannot add more!");

            if (ValidateImage(Image))
                Images.Add(Image);
        }

        /// <summary>
        /// Adds images to the task
        /// </summary>
        /// <param name="Image">The images to be added</param>
        public void AddImages(List<Bitmap> ImagesToAdd)
        {
            if (Images.Count + ImagesToAdd.Count > MaximumImagesCount)
                throw new Exception($"Maksymalna liczba obrazków to: {MaximumImagesCount}!");

            foreach(var Image in ImagesToAdd)
                if (ValidateImage(Image))
                    Images.Add(Image);
        }

        /// <summary>
        /// Removes an image from this task
        /// </summary>
        /// <param name="Image">The image to delete</param>
        public void RemoveImage(Bitmap Image)
        {
            if (Image == null)
                throw new NullReferenceException("Image cannot be null!");

            if (Images.Contains(Image))
                Images.Remove(Image);
        }

        /// <summary>
        /// Updates an image attached to this task
        /// </summary>
        /// <param name="OldImage">The old image to be replaced</param>
        /// <param name="NewImage">New image to be saved</param>
        public void UpdateImage(Bitmap OldImage, Bitmap NewImage)
        {
            if (OldImage == null || NewImage == null)
                throw new NullReferenceException();

            if (!Images.Contains(OldImage))
                return;

            if (ValidateImage(NewImage))
            {
                var index = Images.IndexOf(OldImage);
                Images[index] = NewImage;
            }
        }

        #endregion

        #region Public Helpers

        /// <summary>
        /// Checks if an image is valid and can be added to the task
        /// </summary>
        /// <param name="Image">The image to check</param>
        /// <returns>True if it is fully valid; otherwise, false</returns>
        public static bool ValidateImage(Bitmap Image)
        {
            if (Image == null)
                throw new NullReferenceException("Image cannot be null!");

            if (Image.Width > MaxiumumWidth ||
                Image.Height > MaxiumumHeight)
                throw new Exception($"Obrazek jest za duży. Maksymalny rozmiar to: {MaxiumumWidth}x{MaxiumumHeight}px.");

            return true;

        }

        #endregion

    }
}
