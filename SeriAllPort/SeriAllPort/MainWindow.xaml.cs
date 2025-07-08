using SeriAllPort.Views;
using System.Windows;

namespace SeriAllPort
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Aobut_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow window = new AboutWindow();
            window.Owner = this;
            window.ShowDialog();
        }
    }
}