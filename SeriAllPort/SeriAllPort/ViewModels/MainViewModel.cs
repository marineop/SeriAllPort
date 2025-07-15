using CommonWpf;
using CommonWpf.Communication;
using CommonWpf.Communication.Prococol;
using CommonWpf.Communication.Prococol.EventTypes;
using CommonWpf.Communication.Prococol.PacketModes;
using CommonWpf.Extensions;
using CommonWpf.FileHelper;
using CommonWpf.ViewModels;
using CommonWpf.ViewModels.Log;
using SeriAllPort.ViewModels.Profiles;
using SeriAllPort.ViewModels.Protocols;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace SeriAllPort.ViewModels
{
    public class MainViewModel : ViewModel
    {
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

        private ObservableCollection<Profile> _profiles = new ObservableCollection<Profile>();
        public ObservableCollection<Profile> Profiles
        {
            get => _profiles;
            set
            {
                if (_profiles != value)
                {
                    _profiles = value;
                    OnPropertyChanged();
                }
            }
        }

        private Profile _currentProfile;
        public Profile CurrentProfile
        {
            get => _currentProfile;
            set
            {
                if (_currentProfile != value && value != null)
                {
                    _currentProfile = value;

                    SetSelectedProfileAndProtocolWithId(_currentProfile.Id);

                    OnPropertyChanged();
                }
            }
        }

        private Profile _defaultProfile;

        private ObservableCollection<Protocol> _protocols = new ObservableCollection<Protocol>();
        public ObservableCollection<Protocol> Protocols
        {
            get => _protocols;
            set
            {
                if (_protocols != value)
                {
                    _protocols = value;
                    OnPropertyChanged();
                }
            }
        }

        private Protocol _currentProtocol;
        public Protocol CurrentProtocol
        {
            get => _currentProtocol;
            set
            {
                if (_currentProtocol != value)
                {
                    if (_currentProtocol != null)
                    {
                        _currentProtocol.PacketMode.PacketReceived -= PacketMode_PacketReceived;
                    }

                    _currentProtocol = value;

                    if (_currentProtocol == null)
                    {
                        throw new Exception("_currentProtocol can not be null");
                    }

                    if (_currentProtocol != null)
                    {
                        _currentProtocol.PacketMode.Serial = Serial;

                        _currentProtocol.PacketMode.PacketReceived += PacketMode_PacketReceived;

                        CurrentProfile.ProtocolId = _currentProtocol.Id;
                    }

                    OnPropertyChanged();
                }

                try
                {
                    CurrentProtocolValidation = string.Empty;
                    _currentProtocol?.PacketMode.Validate();
                    CurrentProtocolValidation = string.Empty;
                    CurrentProtocolIsValid = true;
                }
                catch
                {
                    CurrentProtocolIsValid = false;
                    CurrentProtocolValidation = "Invalid";
                }
            }
        }

        private Protocol _defaultProtocol;

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

        private bool _serialIsDisconnected = true;
        public bool SerialIsDisconnected
        {
            get => _serialIsDisconnected;
            set
            {
                if (_serialIsDisconnected != value)
                {
                    _serialIsDisconnected = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _currentProtocolIsValid;
        public bool CurrentProtocolIsValid
        {
            get => _currentProtocolIsValid;
            set
            {
                if (_currentProtocolIsValid != value)
                {
                    _currentProtocolIsValid = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _currentProtocolValidation = string.Empty;
        public string CurrentProtocolValidation
        {
            get => _currentProtocolValidation;
            set
            {
                if (_currentProtocolValidation != value)
                {
                    _currentProtocolValidation = value;
                    OnPropertyChanged();
                }
            }
        }

        private readonly ComPortViewModel _comPortViewModel = new ComPortViewModel();

        public ICommand ProfileEditorCommand { get; private set; }
        public ICommand ProtocolEditorCommand { get; private set; }
        public ICommand SendRawDataCommand { get; private set; }

        public IShowDialog ShowDialog { get; private set; }
        public IShowErrorDialog ShowErrorDialog { get; private set; }

        private readonly AppSettings _appSettings;

        private readonly object _protocolEventLock = new object();

        public MainViewModel(
            IShowDialog showDialog,
            IShowErrorDialog showErrorDialog,
            AppSettings appSettings)
        {
            ShowDialog = showDialog;
            ShowErrorDialog = showErrorDialog;
            _appSettings = appSettings;

            LogViewModel = new LogViewModel();

            ProfileEditorCommand = new SimpleCommand((parameters) => EditProfile());
            ProtocolEditorCommand = new SimpleCommand((parameters) => EditProtocol());
            SendRawDataCommand = new SimpleCommand((parameters) => SendBytes());

            _comPortViewModel.TrySetPortName(_appSettings.LastComPort);
            _comPortViewModel.ComPort.Settings.BaudRate = _appSettings.LastBaudRate;
            _comPortViewModel.ShowDialog = showDialog;

            Serial.PhysicalInterfaces.Add(_comPortViewModel);
            Serial.CurrentInterface = _comPortViewModel;

            Serial.Error += Serial_Error;
            Serial.ConnectionStateChanged += Serial_ConnectionStateChanged;
            Serial.BytesReceived += Serial_BytesReceived;

            try
            {
                Protocols = AppDataFolderFileHelper.LoadFiles<Protocol>();
                Profiles = AppDataFolderFileHelper.LoadFiles<Profile>();

            }
            catch (Exception ex)
            {
                OnError(ex);
            }

            AddDefaultProtocols();
            AddDefaultProfiles();

            SetSelectedProfileAndProtocolWithId(_appSettings.ProfileId);
        }

        public void SendBytes()
        {
            try
            {
                byte[] bytes = CurrentProfile.SendFormats[CurrentProfile.SendFormatIndex].GetBytes();

                if (bytes.Length > 0)
                {
                    Serial.SendBytes(bytes);

                    LogViewModel.AppendLog($"Send    : {bytes.BytesToString()}");
                }
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        public void OnClose()
        {
            _appSettings.LastComPort = _comPortViewModel.ComPort.Settings.PortName;
            _appSettings.LastBaudRate = _comPortViewModel.ComPort.Settings.BaudRate;

            _appSettings.ProfileId = CurrentProfile.Id;

            try
            {
                AppDataFolderFileHelper.SaveFiles(Profiles);
                AppDataFolderFileHelper.SaveFiles(Protocols);
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        private void AddDefaultProtocols()
        {
            // Add common protocols
            if (Protocols.Count <= 0)
            {
                Protocol newProtocol = new Protocol(
                   "CR LF",
                   AppDataFolderFileHelper.GenerateUnusedId(Protocols),
                   new PacketModeEndOfPacketSymbol());

                Protocols.Add(newProtocol);
            }

            // Add Default if not exist
            bool defaultExist = false;
            foreach (Protocol protocol in Protocols)
            {
                if (protocol.Id == Guid.Empty)
                {
                    defaultExist = true;
                    _defaultProtocol = protocol;
                    break;
                }
            }

            if (!defaultExist)
            {
                _defaultProtocol = new Protocol(
                "Default",
                Guid.Empty,
                new PacketModeTimeout());

                _defaultProtocol.CanNotDelete = true;
                _defaultProtocol.CanNotEditName = true;

                Protocols.Add(_defaultProtocol);
            }
        }

        private void AddDefaultProfiles()
        {
            // Add Default if not exist
            bool defaultExist = false;
            foreach (Profile profile in Profiles)
            {
                if (profile.Id == Guid.Empty)
                {
                    defaultExist = true;
                    _defaultProfile = profile;
                    break;
                }
            }

            if (!defaultExist)
            {
                _defaultProfile = new Profile("Default", Guid.Empty);

                _defaultProfile.CanNotDelete = true;
                _defaultProfile.CanNotEditName = true;

                Profiles.Add(_defaultProfile);
            }
        }

        private void SetSelectedProfileAndProtocolWithId(Guid profileId)
        {
            SetupCurrentProfileWithIdOrUseDefault(profileId);

            SetUpProtocolOrUseDefault();

            SetupSettingsAccrodingToCurrentProfile();
        }

        private void SetupCurrentProfileWithIdOrUseDefault(Guid guid)
        {
            bool profileFound = false;
            foreach (Profile profile in Profiles)
            {
                if (profile.Id == guid)
                {
                    _currentProfile = profile;
                    profileFound = true;

                    break;
                }
            }

            if (!profileFound)
            {
                _currentProfile = _defaultProfile;
            }
        }

        private void SetUpProtocolOrUseDefault()
        {
            bool protocolFound = false;
            foreach (Protocol protocol in Protocols)
            {
                if (protocol.Id == CurrentProfile.ProtocolId)
                {
                    CurrentProtocol = protocol;
                    protocolFound = true;

                    break;
                }
            }

            if (!protocolFound)
            {
                CurrentProtocol = _defaultProtocol;
            }
        }

        private void SetupSettingsAccrodingToCurrentProfile()
        {
            _comPortViewModel.ComPort.Settings = CurrentProfile.ComPortSettings;
        }

        private void EditProfile()
        {
            ProfileEditorViewModel profileEditor = new ProfileEditorViewModel(
                Profiles,
                CurrentProfile,
                ShowErrorDialog);
            bool ok = ShowDialog.ShowDialog(profileEditor, "Profile Editor");
            if (ok)
            {
                _defaultProfile = profileEditor.Profiles.First((x) => x.Id == Guid.Empty);

                Profiles = profileEditor.Profiles;

                if (profileEditor.SelectedProfile != null)
                {
                    CurrentProfile = profileEditor.SelectedProfile;
                }
                else
                {
                    CurrentProfile = _defaultProfile;
                }

                try
                {
                    AppDataFolderFileHelper.SaveFiles(Profiles);
                }
                catch (Exception ex)
                {
                    OnError(ex);
                }
            }
        }

        private void EditProtocol()
        {
            ProtocolEditorViewModel protocolEditor = new ProtocolEditorViewModel(
                Protocols,
                CurrentProtocol,
                ShowErrorDialog);
            bool ok = ShowDialog.ShowDialog(protocolEditor, "Protocol Editor");
            if (ok)
            {
                _defaultProtocol = protocolEditor.Protocols.First((x) => x.Id == Guid.Empty);

                if (protocolEditor.SelectedProtocol != null)
                {
                    CurrentProtocol = protocolEditor.SelectedProtocol;
                }
                else
                {
                    CurrentProtocol = _defaultProtocol;
                }

                Protocols = protocolEditor.Protocols;

                try
                {
                    AppDataFolderFileHelper.SaveFiles(Protocols);
                }
                catch (Exception ex)
                {
                    OnError(ex);
                }
            }
        }

        private void Serial_Error(object sender, System.IO.ErrorEventArgs e)
        {
            OnError(e.GetException());
        }

        private void Serial_ConnectionStateChanged(object? sender, CommonWpf.EventHandlers.ConnectionStateChangedEventArgs e)
        {
            SerialIsDisconnected = (e.ConnectionState == ConnectionState.Disconnected);
            if (e.ConnectionState == ConnectionState.Disconnected)
            {
                CurrentProtocol.PacketMode.Terminate();
            }
        }

        private void Serial_BytesReceived(object? sender, EventArgs e)
        {
            CurrentProtocol.PacketMode.BytesReceived();
        }

        private void PacketMode_PacketReceived(object? sender, EventArgs e)
        {
            lock (_protocolEventLock)
            {
                while (true)
                {
                    bool dequeueSuccess = CurrentProtocol.PacketMode.EventQueue.TryDequeue(out PacketEventType? eventNow);
                    if (dequeueSuccess && eventNow != null)
                    {
                        StringBuilder sb = new StringBuilder();
                        byte[] bytes = eventNow.Bytes;

                        if (eventNow is PacketReceived)
                        {
                            sb.Append($"R.Packet: ");

                            bool first = true;

                            if (CurrentProfile.LogDisplayBytes)
                            {
                                sb.Append($"{bytes.BytesToString()}");

                                first = false;
                            }

                            if (CurrentProfile.LogDisplayText)
                            {
                                string text = Encoding.UTF8.GetString(bytes);
                                string cleaned = Regex.Replace(text, @"[^\u0020-\u007E]+", "");
                                sb.Append($"{(first ? "" : "\r\n          ")}{cleaned}");

                                first = false;
                            }
                        }
                        else if (eventNow is NonPacketBytesReceived)
                        {
                            sb.Append($"R.Error : ");
                            sb.Append($"{bytes.BytesToString()}");
                        }

                        LogViewModel.AppendLog($"{sb}");
                    }
                    else
                    {
                        break;
                    }
                }
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

