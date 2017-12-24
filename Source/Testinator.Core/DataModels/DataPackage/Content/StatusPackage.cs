using System;

namespace Testinator.Core
{
    [Serializable]
    public class StatusPackage : PackageContent
    {
        /// <summary>
        /// Indicates if a question has been solved so the server can update status
        /// </summary>
        public bool QuestionSolved { get; set; }
    }
}
