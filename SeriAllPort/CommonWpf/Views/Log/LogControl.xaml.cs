using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace CommonWpf.Views.Log
{
    /// <summary>
    /// Interaction logic for LogControl.xaml
    /// </summary>
    public partial class LogControl : UserControl
    {
        // Dependency Property to expose the collection of user controls
        public static readonly DependencyProperty UserControlsProperty =
            DependencyProperty.Register("UserControls", typeof(ObservableCollection<UIElement>), typeof(LogControl), new PropertyMetadata(new ObservableCollection<UIElement>()));

        public ObservableCollection<UIElement> UserControls
        {
            get { return (ObservableCollection<UIElement>)GetValue(UserControlsProperty); }
            set { SetValue(UserControlsProperty, value); }
        }

        public LogControl()
        {
            InitializeComponent();
        }

        private void MyListView_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Only show context menu if there is at least one selected item
            if (uxLogList.SelectedItems.Count > 0)
            {
                uxContextMenu.PlacementTarget = uxLogList;
                uxContextMenu.IsOpen = true;
            }
        }

        private void uxCopy_Click(object sender, RoutedEventArgs e)
        {
            if (uxLogList.SelectedItems.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (object? item in uxLogList.SelectedItems)
                {
                    sb.AppendLine(item.ToString());
                }

                Clipboard.SetText(sb.ToString().TrimEnd());
            }
        }
    }
}
