using System.IO;

namespace Testinator.Core
{
    /// <summary>
    /// Handles writing logs to the file
    /// </summary>
    public class LogsWriter : FileWriterBase
    {
        /// <summary>
        /// Writes logs to the file
        /// </summary>
        /// <param name="text">Message we want to write</param>
        /// <param name="path">File path</param>
        public override void WriteToFile(string text, string path, bool append = true)
        {
            // Normalize path
            path = NormalizePath(path);

            // Resolve to absolute path
            path = ResolvePath(path);

            // Write the log message to file
            using (var fileStream = (TextWriter)new StreamWriter(File.Open(path, append ? FileMode.Append : FileMode.Create)))
                fileStream.Write(text);
        }
    }
}
