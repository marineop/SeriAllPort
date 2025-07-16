using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommonWpf.Extensions
{
    public static class ComboBoxExtension
    {
        public static readonly DependencyProperty DropDownOpenedCommandProperty =
            DependencyProperty.RegisterAttached(
                "DropDownOpenedCommand",
                typeof(ICommand),
                typeof(ComboBoxExtension),
                new PropertyMetadata(null, OnDropDownOpenedCommandChanged));

        public static void SetDropDownOpenedCommand(DependencyObject element, ICommand value)
        {
            element.SetValue(DropDownOpenedCommandProperty, value);
        }

        public static ICommand GetDropDownOpenedCommand(DependencyObject element)
        {
            return (ICommand)element.GetValue(DropDownOpenedCommandProperty);
        }

        private static void OnDropDownOpenedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ComboBox comboBox)
            {
                if (e.OldValue == null && e.NewValue != null)
                {
                    comboBox.DropDownOpened += ComboBox_DropDownOpened;
                }
                else if (e.OldValue != null && e.NewValue == null)
                {
                    comboBox.DropDownOpened -= ComboBox_DropDownOpened;
                }
            }
        }

        private static void ComboBox_DropDownOpened(object? sender, EventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                ICommand command = GetDropDownOpenedCommand(comboBox);
                if (command != null && command.CanExecute(null))
                {
                    command.Execute(null);
                }
            }
        }
    }
}