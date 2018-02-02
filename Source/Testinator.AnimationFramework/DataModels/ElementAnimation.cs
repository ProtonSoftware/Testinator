namespace Testinator.AnimationFramework
{
    /// <summary>
    /// Every possible element animation as an enum
    /// </summary>
    public enum ElementAnimation
    {
        /// <summary>
        /// No animation takes place
        /// </summary>
        None = 0,

        /// <summary>
        /// The element slides in from the left
        /// </summary>
        SlideFromLeft,

        /// <summary>
        /// The element slides out to the left
        /// </summary>
        SlideToLeft,

        /// <summary>
        /// The element slides in and fades in from the left
        /// </summary>
        SlideAndFadeInFromLeft,

        /// <summary>
        /// The element slides out and fades out to the left
        /// </summary>
        SlideAndFadeOutToLeft,

        /// <summary>
        /// The element slides in from the right
        /// </summary>
        SlideFromRight,

        /// <summary>
        /// The element slides out to the right
        /// </summary>
        SlideToRight,

        /// <summary>
        /// The element slides in and fades in from the right
        /// </summary>
        SlideAndFadeInFromRight,

        /// <summary>
        /// The element slides out and fades out to the right
        /// </summary>
        SlideAndFadeOutToRight,

        /// <summary>
        /// The element slides in from the top
        /// </summary>
        SlideFromTop,

        /// <summary>
        /// The element slides out to the top
        /// </summary>
        SlideToTop,

        /// <summary>
        /// The element slides in and fades in from the top
        /// </summary>
        SlideAndFadeInFromTop,

        /// <summary>
        /// The element slides out and fades out to the top
        /// </summary>
        SlideAndFadeOutToTop,

        /// <summary>
        /// The element slides in from the bottom
        /// </summary>
        SlideFromBottom,

        /// <summary>
        /// The element slides out to the bottom
        /// </summary>
        SlideToBottom,

        /// <summary>
        /// The element slides in and fades in from the bottom
        /// </summary>
        SlideAndFadeInFromBottom,

        /// <summary>
        /// The element slides out and fades out to the bottom
        /// </summary>
        SlideAndFadeOutToBottom,

        /// <summary>
        /// The element slides in and fades in from undefined at this stage direction
        /// </summary>
        SlideAndFadeInMixed,

        /// <summary>
        /// The element slides out and fades out to undefined at this stage direction
        /// </summary>
        SlideAndFadeOutMixed,
    }
}
