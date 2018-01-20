using System.IO;

namespace Testinator.Core
{
    /// <summary>
    /// The base for every file writer
    /// </summary>
    public abstract class FileWriterBase : FileManagerBase
    {
        #region File Writers Methods

        /// <summary>
        /// Writes the <see cref="Test"/> to the file
        /// </summary>
        public virtual void WriteToFile(Test test) { }

        /// <summary>
        /// Writes the <see cref="GradingPercentage"/> to the file
        /// </summary>
        public virtual void WriteToFile(string filename, GradingPercentage grading) { }

        /// <summary>
        /// Writes logs to the file
        /// </summary>
        public virtual void WriteToFile(string text, string path, bool append = true) { }

        /// <summary>
        /// Writes <see cref="TestResults"/> to file
        /// </summary>
        public virtual void WriteToFile(TestResults tr) { }

        /// <summary>
        /// Writes <see cref="ClientTestResults"/> to file
        /// </summary>
        public virtual void WriteToFile(ClientTestResults ctr) { }

        /// <summary>
        /// Writes object's property info to file
        /// </summary>
        public virtual void WriteToFile(object property, bool fileExists = true) { }

        /// <summary>
        /// Deletes the <see cref="Test"/>
        /// </summary>
        public virtual void DeleteFile(Test test) { }

        /// <summary>
        /// Deletes the <see cref="TestResults"/>
        /// </summary>
        public virtual void DeleteFile(TestResults results) { }

        #endregion

        #region Protected Helpers

        /// <summary>
        /// Replaces any / with \
        /// </summary>
        /// <param name="path">The path to normalize</param>
        protected string NormalizePath(string path) => path?.Replace('/', '\\').Trim();

        /// <summary>
        /// Resolves any relative elements of the path to absolute
        /// </summary>
        /// <param name="path">The path to resolve</param>
        protected string ResolvePath(string path) => Path.GetFullPath(path);

        #endregion
    }
}
