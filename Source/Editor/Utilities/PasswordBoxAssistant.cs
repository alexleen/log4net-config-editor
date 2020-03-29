// Copyright © 2020 Alex Leendertsen

using System.Windows;
using System.Windows.Controls;

namespace Editor.Utilities
{
    /// <summary>
    /// Allows binding of PasswordBox password.
    /// Adapted from http://blog.functionalfun.net/2008/06/wpf-passwordbox-and-data-binding.html.
    /// </summary>
    public class PasswordBoxAssistant
    {
        public static readonly DependencyProperty BoundPassword = DependencyProperty.RegisterAttached("BoundPassword",
                                                                                                      typeof(string),
                                                                                                      typeof(PasswordBoxAssistant),
                                                                                                      new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

        private static bool sUpdatingPassword;

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is PasswordBox box))
            {
                return;
            }

            // Avoid recursive updating by ignoring the box's changed event
            box.PasswordChanged -= HandlePasswordChanged;

            string newPassword = (string)e.NewValue;

            if (!sUpdatingPassword)
            {
                box.Password = newPassword;
            }

            box.PasswordChanged += HandlePasswordChanged;
        }

        private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!(sender is PasswordBox box))
            {
                return;
            }

            // Set a flag to indicate that we're updating the password - this disables a push back to the PasswordBox - which already has the value
            sUpdatingPassword = true;

            // Push the new password into the BoundPassword property (the source property defined in the XAML)
            SetBoundPassword(box, box.Password);

            sUpdatingPassword = false;
        }

        public static string GetBoundPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(BoundPassword);
        }

        public static void SetBoundPassword(DependencyObject dp, string value)
        {
            dp.SetValue(BoundPassword, value);
        }
    }
}
