namespace Testinator.Server.Core
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
                case IconType.Home:
                    return "\uf015";

                case IconType.Settings:
                    return "\uf013";

                case IconType.Screen:
                    return "\uf108";

                case IconType.InfoCircle:
                    return "\uf05a";

                case IconType.Test:
                    return "\uf016";

                case IconType.Editor:
                    return "\uf044";

                case IconType.MultipleChoiceQuestion:
                    return "\uf031";

                case IconType.MultipleCheckboxesQuestion:
                    return "\uf046";

                case IconType.SingleTextBoxQuestion:
                    return "\uf096";

                case IconType.Check:
                    return "\uf00c";

                case IconType.UnCheck:
                    return "\uf00d";

                // If none found, return null
                default:
                    return null;
            }
        }
    }
}
