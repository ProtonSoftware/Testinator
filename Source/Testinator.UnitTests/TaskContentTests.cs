using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using Testinator.Core;

namespace Testinator.UnitTests
{
    [TestClass]
    public class TaskContentTests
    {

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CreatingTaskContent_NullStringContent()
        {
            var task = new TaskContent(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CreatingTaskContent_EmptyStringContent()
        {
            var task = new TaskContent("");
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AddingTask_NullTask()
        {
            var task = new TaskContent("something");

            task.AddStringContent(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AddingTask_EmptyTask()
        {
            var task = new TaskContent("something");

            task.AddStringContent("");
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AddingImage_NullImage()
        {
            var task = new TaskContent("something");

            task.AddImage(null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AddingImage_TooBigVertiacalResolution()
        {
            var task = new TaskContent("something");
            var image = new Bitmap(TaskContent.MaxiumumWidth, TaskContent.MaxiumumHeight + 1);

            task.AddImage(image);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AddingImage_TooBigHorizontalResolution()
        {
            var task = new TaskContent("something");
            var image = new Bitmap(TaskContent.MaxiumumWidth + 1, TaskContent.MaxiumumHeight);

            task.AddImage(image);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AddingImage_TooBigResolution()
        {
            var task = new TaskContent("something");
            var image = new Bitmap(TaskContent.MaxiumumWidth + 1, TaskContent.MaxiumumHeight + 1);

            task.AddImage(image);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AddingImage_MaximumNumberOfImagesReached()
        {
            var task = new TaskContent("something");
            var image = new Bitmap(TaskContent.MaxiumumWidth, TaskContent.MaxiumumHeight);

            for (var i = 0; i < TaskContent.MaximumImagesCount; i++)
                task.AddImage(image);

            task.AddImage(image);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void RemovingImage_NullArgument()
        {
            var task = new TaskContent("something");
            
            task.RemoveImage(null);
        }

        [TestMethod]
        public void RemovingImage_NonExistingImage()
        {
            var task = new TaskContent("something");
            var oldCount = task.Images.Count;
            task.RemoveImage(new Bitmap(11,11));
            var newCount = task.Images.Count;

            Assert.AreEqual(oldCount, newCount);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdatingImage_NullOldImage()
        {
            var task = new TaskContent("something");
            task.UpdateImage(null, new Bitmap(11, 11));

        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdatingImage_NullNewImage()
        {
            var task = new TaskContent("something");
            task.UpdateImage(new Bitmap(11, 11), null);
        }

        [TestMethod]
        public void UpdatingImage_NotExistingOldImage()
        {
            var task = new TaskContent("something");
            var OldImage = new Bitmap(11, 11);
            var NewImage = new Bitmap(11, 11);
            task.UpdateImage(OldImage, NewImage);
            Assert.IsFalse(task.Images.Contains(NewImage));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void UpdatingImage_NewImageTooBig()
        {
            var task = new TaskContent("something");
            var OldImage = new Bitmap(11, 11);
            var NewImage = new Bitmap(TaskContent.MaxiumumWidth + 1, TaskContent.MaxiumumHeight + 1);
            task.AddImage(OldImage);
            task.UpdateImage(OldImage, NewImage);
        }

        [TestMethod]
        public void UpdatingImage_NewFullyGood()
        {
            var task = new TaskContent("something");
            var OldImage = new Bitmap(11, 11);
            var NewImage = new Bitmap(TaskContent.MaxiumumWidth, TaskContent.MaxiumumHeight);
            task.AddImage(OldImage);
            var IndexOfOldImage = task.Images.IndexOf(OldImage);
            task.UpdateImage(OldImage, NewImage);
            Assert.AreEqual(NewImage, task.Images[IndexOfOldImage]);
        }
    }
}
