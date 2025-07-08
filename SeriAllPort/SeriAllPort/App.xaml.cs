using CommonWpf;
using SeriAllPort.ViewModels;
using System.Windows;

namespace SeriAllPort
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IShowErrorDialog
    {
        public static AppSettings AppSettings { get; private set; } = new AppSettings();

        private readonly string _settingsPath = $"SeriAllPort\\settings.bin";

        private readonly MainViewModel _mainViewModel;

        public App()
        {
            AppSettings = FileSerializer<AppSettings>.LoadFromAppDataPath(_settingsPath);

            _mainViewModel = new MainViewModel(this, AppSettings);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow mainWindow = new MainWindow();
            mainWindow.DataContext = _mainViewModel;

            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            _mainViewModel.OnClose();

            FileSerializer<AppSettings>.SaveToAppDataPath(AppSettings, _settingsPath);
        }

        public void ShowError(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
