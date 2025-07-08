using System.Windows;

namespace SeriAllPort.Views
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void uxOk_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
