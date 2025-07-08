using CommonWpf.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace CommonWpf.Views.ComPort
{
    /// <summary>
    /// Interaction logic for ComPortConnectionControl.xaml
    /// </summary>
    public partial class ComPortConnectionControl : UserControl, IShowDialog
    {
        public ComPortConnectionControl()
        {
            InitializeComponent();
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is not null and ComPortViewModel)
            {
                if (e.NewValue is ComPortViewModel viewModel)
                {
                    viewModel.ShowDialog = this;
                }
            }
        }

        public bool ShowDialog(object dataContext)
        {
            ComPortSettingsWindow window = new ComPortSettingsWindow();
            window.Owner = Window.GetWindow(this);
            window.DataContext = dataContext;
            bool? ok = window.ShowDialog();
            return ok ?? false;
        }
    }
}
