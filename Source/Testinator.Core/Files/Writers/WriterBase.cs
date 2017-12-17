namespace Testinator.Core
{
    public abstract class WriterBase
    {
        /// <summary>
        /// The settings for this writer
        /// </summary>
        protected WriterSettings Settings { get; set; }

        /// <summary>
        /// Writes the data 
        /// </summary>
        /// <param name="FileName">The name of the file</param>
        /// <param name="data">Data to be saved</param>
        public abstract void Write(string FileName, object data);
    }
}
