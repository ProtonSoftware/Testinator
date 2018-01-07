using System.IO;

namespace Testinator.Core
{
    /// <summary>
    /// The base for every file writer
    /// </summary>
    public abstract class FileWriterBase
    {
        #region Protected Properties

        /// <summary>
        /// The settings for this writer
        /// </summary>
        protected WriterSettings Settings { get; set; }

        #endregion

        #region File Writers Methods

        /// <summary>
        /// Writes <see cref="Test"/> to the file
        /// </summary>
        public virtual void WriteToFile(Test test) { }

        /// <summary>
        /// Writes <see cref="GradingPercentage"/> to the file
        /// </summary>
        public virtual void WriteToFile(string fileName, GradingPercentage grading) { }

        /// <summary>
        /// Writes logs to the file
        /// </summary>
        public virtual void WriteToFile(string text, string path, bool append = true) { }

        /// <summary>
        /// Writes test results to file
        /// </summary>
        public virtual void WriteToFile(TestResults tr) { }

        /// <summary>
        /// Writes client test results to file
        /// </summary>
        public virtual void WriteToFile(ClientTestResults ctr) { }

        /// <summary>
        /// Deletes a <see cref="Test"/>
        /// </summary>
        public virtual void DeleteFile(Test test) { }

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
