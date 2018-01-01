using System;
using System.Collections.Generic;

namespace Testinator.Core
{
    [Serializable]
    public class ResultFormPackage : PackageContent
    {
        /// <summary>
        /// The answers given by the user
        /// </summary>
        public List<Answer> Answers { get; set; }

        /// <summary>
        /// Points scored by the user
        /// </summary>
        public int PointsScored { get; set; }

        /// <summary>
        /// The client mark
        /// </summary>
        public Marks Mark { get; set; }
    }
}
