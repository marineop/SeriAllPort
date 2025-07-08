using CommonWpf;
using CommonWpf.Extensions;
using CommonWpf.ViewModels;
using CommonWpf.ViewModels.Log;
using SeriAllPort.ViewModels.SendRawData;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace SeriAllPort.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private SerialViewModel _serial = new SerialViewModel();
        public SerialViewModel Serial
        {
            get => _serial;
            set
            {
                if (_serial != value)
                {
                    _serial = value;
                    OnPropertyChanged();
                }
            }
        }

        private ITextToBytes _SendFormat;
        public ITextToBytes SendFormat
        {
            get => _SendFormat;
            set
            {
                if (_SendFormat != value)
                {
                    _SendFormat = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<ITextToBytes> _SendFormats;
        public ObservableCollection<ITextToBytes> SendFormats
        {
            get => _SendFormats;
            set
            {
                if (_SendFormats != value)
                {
                    _SendFormats = value;
                    OnPropertyChanged();
                }
            }
        }

        public LogViewModel LogViewModel { get; set; }

        private string _lastErrorMessage = string.Empty;
        public string LastErrorMessage
        {
            get => _lastErrorMessage;
            set
            {
                if (_lastErrorMessage != value)
                {
                    _lastErrorMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SendRawDataCommand { get; private set; }

        public IShowErrorDialog ShowErrorDialog { get; private set; }

        private readonly AppSettings _appSettings;

        public MainViewModel(IShowErrorDialog showErrorDialog, AppSettings appSettings)
        {
            ShowErrorDialog = showErrorDialog;

            _SendFormats = new ObservableCollection<ITextToBytes>()
            {
                new SendFormatBytes(),
                new SendFormatString()
            };

            _SendFormat = SendFormats[0];

            LogViewModel = new LogViewModel();

            SendRawDataCommand = new SimpleCommand((parameters) => SendBytes());

            _appSettings = appSettings;
            Serial.ComPortViewModel.TrySetPortName(_appSettings.LastComPort);
            Serial.ComPortViewModel.ComPort.Settings.BaudRate = _appSettings.LastBaudRate;

            Serial.CurrentInterface.Error += CurrentInterface_Error;
            Serial.CurrentInterface.BytesReceived += CurrentInterface_BytesReceived;
        }

        public void OnClose()
        {
            _appSettings.LastComPort = Serial.ComPortViewModel.ComPort.Settings.PortName;
            _appSettings.LastBaudRate = Serial.ComPortViewModel.ComPort.Settings.BaudRate;
        }

        private void CurrentInterface_BytesReceived(object? sender, CommonWpf.EventHandlers.BytesReceivedEventArgs e)
        {
            LogViewModel.AppendLog($"Receive: {e.Bytes.BytesToString()}");
        }

        private void CurrentInterface_Error(object sender, System.IO.ErrorEventArgs e)
        {
            OnError(e.GetException());
        }

        public void SendBytes()
        {
            try
            {
                byte[] bytes = SendFormat.GetBytes();
                Serial.CurrentInterface.SendBytes(bytes);
                LogViewModel.AppendLog($"Send   : {bytes.BytesToString()}");
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        private void OnError(Exception exception)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                LastErrorMessage = exception.Message;
                LogViewModel.AppendLog($"Error: {LastErrorMessage}");
                ShowErrorDialog.ShowError("Error", LastErrorMessage);
            });
        }
    }
}
