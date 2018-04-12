using System;

namespace Testinator.Core
{
    /// <summary>
    /// The scoring for every question
    /// </summary>
    [Serializable]
    public class Scoring
    {
        #region Public Properties

        /// <summary>
        /// Scoring mode used for evaluating
        /// </summary>
        public ScoringMode Mode { get; set; }

        /// <summary>
        /// Number of points given for a fully correct answer
        /// </summary>
        public int FullPointScore { get; private set; }

        /// <summary>
        /// Indicates if the scoring system should be case sensitive
        /// NOTE: Makes sense only in specific questions like e.g. SingleTextBoxes
        /// Can be interpreted differently depends on a question type
        /// </summary>
        public bool IsCaseSensitive { get; set; }

        /// <summary>
        /// Indicates if whitespaces are important while evaluating
        /// NOTE: Makes sense only in specific questions like e.g. SingleTextBoxes
        /// Can be interpreted differently depends on a question type
        /// </summary>
        public bool AreWhiteSpacesImportant { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates scoring object with the given full score and mode
        /// </summary>
        /// <param name="CurrentMode">Current scoring mode</param>
        /// <param name="FullScore">Full score points</param>
        /// <param name="CaseSensitive">Indicates if this criteria is case sensitive. Makes sense in specific contexts
        /// False by default.</param>
        public Scoring(ScoringMode CurrentMode, int FullScore)
        {
            if (FullScore < 1)
                throw new Exception("Liczba punktów nie może być mniejsza od 1!");

            Mode = CurrentMode;
            FullPointScore = FullScore;
        }

        #endregion
    }
}
