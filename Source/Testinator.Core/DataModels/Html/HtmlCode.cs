using System;

namespace Testinator.Core
{
    /// <summary>
    /// Representation of html code
    /// </summary>
    [Serializable]
    public class HtmlCode
    {

        private string HtmlString { get; set; }

        public override string ToString()
        {
            return HtmlString;
        }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(HtmlString);
        }

        public HtmlCode()
        {

        }

        public HtmlCode(string Html)
        {
            HtmlString = Html;
        }
    }
}
