using System;

namespace Testinator.Core
{
    [Serializable]
    public class StatusPackage : PackageContent
    {
        /// <summary>
        /// The question number the client is solving right now
        /// </summary>
        public int CurrentQuestion { get; set; }
    }
}
