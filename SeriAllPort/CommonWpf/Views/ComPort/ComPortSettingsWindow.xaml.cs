using System.Windows;

namespace CommonWpf.Views.ComPort
{
    /// <summary>
    /// Interaction logic for ComPortSettingsWindow.xaml
    /// </summary>
    public partial class ComPortSettingsWindow : Window
    {
        public ComPortSettingsWindow()
        {
            InitializeComponent();
        }

        private void uxOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void uxCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
