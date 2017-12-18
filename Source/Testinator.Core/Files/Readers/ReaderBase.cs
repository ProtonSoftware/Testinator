using System.Collections.Generic;

namespace Testinator.Core
{
    public abstract class ReaderBase
    {
        /// <summary>
        /// The settings for this reader
        /// </summary>
        protected ReaderSettings Settings { get; set; }
    }
}
