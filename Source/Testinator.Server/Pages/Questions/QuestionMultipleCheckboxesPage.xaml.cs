﻿using Testinator.AnimationFramework;
using Testinator.Server.Core;
using Testinator.UICore;

namespace Testinator.Server
{
    /// <summary>
    /// Interaction logic for QuestionMultipleCheckboxesPage.xaml
    /// </summary>
    public partial class QuestionMultipleCheckboxesPage : BasePage<QuestionMultipleCheckboxesViewModel>
    {
        #region Constructor

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public QuestionMultipleCheckboxesPage(QuestionMultipleCheckboxesViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();

            // Set default animations
            PageLoadAnimation = ElementAnimation.SlideAndFadeInFromRight;
            PageUnloadAnimation = ElementAnimation.SlideAndFadeOutToLeft;
        }

        #endregion
    }
}
