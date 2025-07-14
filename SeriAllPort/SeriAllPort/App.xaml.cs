using CommonWpf;
using CommonWpf.FileHelper;
using CommonWpf.Views;
using SeriAllPort.ViewModels;
using System.Windows;

namespace SeriAllPort
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IShowDialog, IShowErrorDialog
    {
        public static AppSettings AppSettings { get; private set; }

        private const string _settingsPath = $"settings.bin";

        private readonly MainViewModel _mainViewModel;

        private MainWindow? _mainWindow;

        static App()
        {
            FileSerializer.AppName = "SeriAllPort";

            AppSettings? temp = null;
            try
            {
                temp = FileSerializer.LoadFromAppDataPath<AppSettings>(_settingsPath);
            }
            catch
            {
            }

            if (temp == null)
            {
                AppSettings = new AppSettings();
            }
            else
            {
                AppSettings = temp;
            }
        }

        public App()
        {
            _mainViewModel = new MainViewModel(this, this, AppSettings);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _mainWindow = new MainWindow();
            _mainWindow.DataContext = _mainViewModel;

            _mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            _mainViewModel.OnClose();

            try
            {
                FileSerializer.SaveToAppDataPath(AppSettings, _settingsPath);
            }
            catch
            {
            }
        }

        public bool ShowDialog(object dataContext, string title)
        {
            DialogWindow window = new DialogWindow();
            window.Title = title;
            window.Owner = Window.GetWindow(_mainWindow);
            window.DataContext = dataContext;
            bool? ok = window.ShowDialog();

            return ok ?? false;
        }

        public void ShowError(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
