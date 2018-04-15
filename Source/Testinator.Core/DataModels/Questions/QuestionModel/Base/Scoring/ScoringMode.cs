namespace Testinator.Core
{
    /// <summary>
    /// The possible scoring models
    /// NOTE: can be interpret differently for each question
    /// </summary>
    public enum ScoringMode
    {
        /// <summary>
        /// Points can be given only for a fully correct answer
        /// </summary>
        FullAnswer,
        
        /// <summary>
        /// Points can be given for a half good answer
        /// </summary>
        HalfTheAnswer,

        /// <summary>
        /// Points can be given evenly for each correct part of the answer
        /// </summary>
        EvenParts,
    }
}
