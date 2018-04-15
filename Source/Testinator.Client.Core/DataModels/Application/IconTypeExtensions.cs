namespace Testinator.Client.Core
{
    /// <summary>
    /// Helper functions for <see cref="IconType"/>
    /// </summary>
    public static class IconTypeExtensions
    {
        /// <summary>
        /// Converts <see cref="IconType"/> to a FontAwesome string
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ToFontAwesome(this IconType type)
        {
            switch (type)
            {
                case IconType.MultipleChoiceQuestion:
                    return "\uf031";

                case IconType.MultipleCheckBoxesQuestion:
                    return "\uf046";

                case IconType.SingleTextBoxQuestion:
                    return "\uf096";

                // If none found, return null
                default:
                    return null;
            }
        }
    }
}
