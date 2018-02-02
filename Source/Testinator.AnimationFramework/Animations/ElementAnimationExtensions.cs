using System.Threading.Tasks;
using System.Windows;

namespace Testinator.AnimationFramework
{
    /// <summary>
    /// Extensions to animate elements in or out
    /// Used to animate something without attached property mechanic, simply in C# code
    /// </summary>
    public static class ElementAnimationExtensions
    {
        /// <summary>
        /// Animates the <see cref="FrameworkElement"/> in by specified animation
        /// </summary>
        /// <param name="element">The element to animate</param>
        /// <param name="animation">The type of animation to do</param>
        /// <param name="time">The time animation will take</param>
        /// <returns></returns>
        public static async Task AnimateInAsync(this FrameworkElement element, ElementAnimation animation, float time = 0.7f)
        {
            // Make sure we have something to do
            if (animation == ElementAnimation.None)
                return;

            // Get application size to know how big animation will be
            var appSize = (int)Application.Current.MainWindow.Width;

            // Based on animation type...
            switch (animation)
            {
                // Start desired animation
                case ElementAnimation.SlideFromLeft:
                    await element.SlideInAsync(AnimationSlideInDirection.Left, false, time, size: appSize);
                    break;
                case ElementAnimation.SlideAndFadeInFromLeft:
                    await element.SlideAndFadeInAsync(AnimationSlideInDirection.Left, false, time, size: appSize);
                    break;
                case ElementAnimation.SlideFromRight:
                    await element.SlideInAsync(AnimationSlideInDirection.Right, false, time, size: appSize);
                    break;
                case ElementAnimation.SlideAndFadeInFromRight:
                    await element.SlideAndFadeInAsync(AnimationSlideInDirection.Right, false, time, size: appSize);
                    break;
                case ElementAnimation.SlideFromTop:
                    await element.SlideInAsync(AnimationSlideInDirection.Top, false, time, size: appSize);
                    break;
                case ElementAnimation.SlideAndFadeInFromTop:
                    await element.SlideAndFadeInAsync(AnimationSlideInDirection.Top, false, time, size: appSize);
                    break;
                case ElementAnimation.SlideFromBottom:
                    await element.SlideInAsync(AnimationSlideInDirection.Bottom, false, time, size: appSize);
                    break;
                case ElementAnimation.SlideAndFadeInFromBottom:
                    await element.SlideAndFadeInAsync(AnimationSlideInDirection.Bottom, false, time, size: appSize);
                    break;
            }
        }

        /// <summary>
        /// Animates the <see cref="FrameworkElement"/> out by specified animation
        /// </summary>
        /// <param name="element">The element to animate</param>
        /// <param name="animation">The type of animation to do</param>
        /// <param name="time">The time animation will take</param>
        /// <returns></returns>
        public static async Task AnimateOutAsync(this FrameworkElement element, ElementAnimation animation, float time = 0.7f)
        {
            // Make sure we have something to do
            if (animation == ElementAnimation.None)
                return;

            // Based on animation type...
            switch (animation)
            {
                // Start desired animation
                case ElementAnimation.SlideToLeft:
                    await element.SlideOutAsync(AnimationSlideInDirection.Left, time);
                    break;

                case ElementAnimation.SlideAndFadeOutToLeft:
                    await element.SlideAndFadeOutAsync(AnimationSlideInDirection.Left, time);
                    break;

                case ElementAnimation.SlideToRight:
                    await element.SlideOutAsync(AnimationSlideInDirection.Right, time);
                    break;

                case ElementAnimation.SlideAndFadeOutToRight:
                    await element.SlideAndFadeOutAsync(AnimationSlideInDirection.Right, time);
                    break;

                case ElementAnimation.SlideToTop:
                    await element.SlideOutAsync(AnimationSlideInDirection.Top, time);
                    break;

                case ElementAnimation.SlideAndFadeOutToTop:
                    await element.SlideAndFadeOutAsync(AnimationSlideInDirection.Top, time);
                    break;

                case ElementAnimation.SlideToBottom:
                    await element.SlideOutAsync(AnimationSlideInDirection.Bottom, time);
                    break;

                case ElementAnimation.SlideAndFadeOutToBottom:
                    await element.SlideAndFadeOutAsync(AnimationSlideInDirection.Bottom, time);
                    break;
            }
        }
    }
}