using System.Drawing;
using Testinator.Core;

namespace Testinator.Server
{
    /// <summary>
    /// Viewmodel for a images editor item
    /// </summary>
    public class ImagesEditorItemViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Id of this item in list
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The thumbnail for this item
        /// </summary>
        public Image Thumbnail { get; set; }

        #endregion
    }
}
