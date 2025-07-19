using CommonWpf;
using CommonWpf.FileHelper;
using CommonWpf.Views;
using SeriAllPort.ViewModels;
using System.Windows;
using System.Windows.Threading;

namespace SeriAllPort
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IShowDialog, IShowErrorDialog
    {
        public static AppSettings AppSettings { get; private set; }

        private const string _settingsPath = $"settings.bin";

        private MainViewModel? _mainViewModel;

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
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception? ex = e.ExceptionObject as Exception;
            ShowError("AppDomain.CurrentDomain.UnhandledException", ex?.Message);
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            ShowError("DispatcherUnhandledException", e.Exception.ToString());
        }

        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
            ShowError("TaskScheduler.UnobservedTaskException", e.Exception.ToString());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _mainWindow = new MainWindow();

            _mainViewModel = new MainViewModel(this, this, AppSettings);

            _mainWindow.DataContext = _mainViewModel;

            _mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            _mainViewModel?.OnClose();

            try
            {
                FileSerializer.SaveToAppDataPath(AppSettings, _settingsPath);
            }
            catch
            {
            }
        }

        public bool ShowDialog(
            object dataContext,
            string title,
            ResizeMode resizeMode,
            SizeToContent sizeToContent,
            bool showInTaskbar)
        {
            DialogWindow window = new DialogWindow();
            window.Title = title;
            window.ResizeMode = resizeMode;
            window.SizeToContent = sizeToContent;
            window.ShowInTaskbar = showInTaskbar;
            window.Owner = Window.GetWindow(_mainWindow);
            window.DataContext = dataContext;

            bool? ok = window.ShowDialog();

            return ok ?? false;
        }

        public void ShowError(string? title, string? message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
