using CommonWpf.Communication;
using System.Windows;
using System.Windows.Controls;

namespace CommonWpf.Views
{
    /// <summary>
    /// Interaction logic for ConnectionStateControl.xaml
    /// </summary>
    public partial class ConnectionStateControl : UserControl
    {
        public ConnectionState ConnectionState
        {
            get { return (ConnectionState)GetValue(ConnectionStateProperty); }
            set { SetValue(ConnectionStateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConnectionState.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConnectionStateProperty =
            DependencyProperty.Register(
                "ConnectionState",
                typeof(ConnectionState),
                typeof(ConnectionStateControl),
                new PropertyMetadata(ConnectionState.Disconnected));

        public ConnectionStateControl()
        {
            InitializeComponent();
        }
    }
}
