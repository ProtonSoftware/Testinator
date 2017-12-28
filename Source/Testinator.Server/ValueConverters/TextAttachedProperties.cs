using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Testinator.UICore;

namespace Testinator.Server
{
    /// <summary>
    /// Focuses (keyboard focus) this element on load
    /// </summary>
    public class IsFocusedProperty : BaseAttachedProperty<IsFocusedProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // If we don't have a control, return
            if (!(sender is Control control))
                return;

            // Focus this control once loaded
            control.Loaded += (s, se) => control.Focus();
        }
    }

    /// <summary>
    /// Focuses (keyboard focus) and selects all text in this element if true
    /// </summary>
    public class FocusAndSelectProperty : BaseAttachedProperty<FocusAndSelectProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TextBoxBase control)
            {
                if ((bool)e.NewValue)
                {
                    // Focus this control
                    control.Focus();

                    // Select all text
                    control.SelectAll();
                }
            }
            if (sender is PasswordBox password)
            {
                if ((bool)e.NewValue)
                {
                    // Focus this control
                    password.Focus();

                    // Select all text
                    password.SelectAll();
                }
            }
        }
    }

    /// <summary>
    /// Used on textbox prevents it from accepting non-number values
    /// </summary>
    public class IntegerOnlyTextBoxProperty : BaseAttachedProperty<IntegerOnlyTextBoxProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // If sender isn't textbox, return
            if (!(sender is TextBoxBase textBox))
                return;

            // Everytime text inside changes...
            textBox.PreviewTextInput += TextBox_PreviewTextInput;
        }

        /// <summary>
        /// Fired everytime has inside textbox has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                // Try to convert text to the int
                int.Parse(e.Text);
            }
            catch
            {
                // Convertion has failed, that means textbox has non-number characters inside
                // So don't add anything, mark this event as handled already
                e.Handled = true;
            }
        }
    }
}
