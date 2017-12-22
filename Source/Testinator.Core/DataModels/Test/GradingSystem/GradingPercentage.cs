namespace Testinator.Core
{
    /// <summary>
    /// Grading (criteria) system which border values are in percents
    /// This class should be converted to <see cref="GradingPoints"/> and then attached to <see cref="Test"/>
    /// </summary>
    public class GradingPercentage
    {
        #region Public Properties 

        /// <summary>
        /// Name of this grading set
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Indicates if <see cref="Marks.A"/> is included in this set
        /// </summary>
        public bool IsMarkAIncluded { get; set; }

        public Mark MarkA { get; set; }

        public Mark MarkB { get; set; }

        public Mark MarkC { get; set; }

        public Mark MarkD { get; set; }

        public Mark MarkE { get; set; }

        public Mark MarkF { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public GradingPercentage()
        {
            // Set default values
            IsMarkAIncluded = true;
            UpdateMark(Marks.A, 100, 96);
            UpdateMark(Marks.B, 95, 86);
            UpdateMark(Marks.C, 85, 70);
            UpdateMark(Marks.D, 69, 50);
            UpdateMark(Marks.E, 49, 30);
            UpdateMark(Marks.F, 29, 0);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates values of a mark
        /// </summary>
        /// <param name="mark">The mark which values we are updating</param>
        /// <param name="top">The top limit in points</param>
        /// <param name="bottom">The bottom limit in points</param>
        public void UpdateMark(Marks mark, int top, int bottom)
        {
            // Based on mark...
            switch(mark)
            {
                // Update top and bottom values
                case Marks.A:
                    {
                        MarkA.TopLimit = top;
                        MarkA.BottomLimit = top;
                    }
                    break;
                case Marks.B:
                    {
                        MarkB.TopLimit = top;
                        MarkB.BottomLimit = top;
                    }
                    break;
                case Marks.C:
                    {
                        MarkC.TopLimit = top;
                        MarkC.BottomLimit = top;
                    }
                    break;
                case Marks.D:
                    {
                        MarkD.TopLimit = top;
                        MarkD.BottomLimit = top;
                    }
                    break;
                case Marks.E:
                    {
                        MarkE.TopLimit = top;
                        MarkE.BottomLimit = top;
                    }
                    break;
                case Marks.F:
                    {
                        MarkF.TopLimit = top;
                        MarkF.BottomLimit = top;
                    }
                    break;
            }
        }

        #endregion
    }
}
