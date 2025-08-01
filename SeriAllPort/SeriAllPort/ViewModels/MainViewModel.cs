﻿
using CommonWpf;
using CommonWpf.Communication;
using CommonWpf.Communication.Protocol;
using CommonWpf.Communication.Protocol.EventTypes;
using CommonWpf.Communication.Protocol.PacketFields;
using CommonWpf.Communication.Protocol.PacketModes;
using CommonWpf.Communication.Protocol.ParseData;
using CommonWpf.Extensions;
using CommonWpf.FileHelper;
using CommonWpf.ViewModels;
using CommonWpf.ViewModels.Log;
using SeriAllPort.ViewModels.Commands;
using SeriAllPort.ViewModels.Profiles;
using SeriAllPort.ViewModels.Protocols;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace SeriAllPort.ViewModels
{
    public partial class MainViewModel : ViewModel
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

        private ObservableCollection<Profile> _profiles = [];
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
                if (value != null)
                {
                    _currentProfile = value;

                    SetSelectedProfileAndProtocolWithId(_currentProfile.Id);

                    OnPropertyChanged();
                }
            }
        }

        private Profile _defaultProfile = new Profile(string.Empty, Guid.Empty);

        private ObservableCollection<Protocol> _protocols = [];
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
                if (value != null)
                {
                    if (_currentProtocol != null)
                    {
                        _currentProtocol.PacketMode.DataReceived -= PacketMode_PacketReceived;
                    }

                    _currentProtocol = value;

                    _currentProtocol.PacketMode.Serial = Serial;
                    _currentProtocol.PacketMode.DataReceived += PacketMode_PacketReceived;

                    CurrentProfile.ProtocolId = _currentProtocol.Id;

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

                    OnPropertyChanged();
                }
            }
        }

        private Protocol _defaultProtocol = new Protocol(string.Empty, Guid.Empty, new PacketModeTimeout());

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

                    ProtocolEditorCommand.RaiseCanExecuteChangedEvent();

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

        private ObservableCollection<Command> Commands => CurrentProfile.Commands;

        private readonly ComPortViewModel _comPortViewModel = new ComPortViewModel();

        public SimpleCommand ProfileEditorCommand { get; private set; }
        public SimpleCommand ProtocolEditorCommand { get; private set; }
        public SimpleCommand CommandEditorCommand { get; private set; }
        public SimpleCommand CrcCalculatorCommand { get; private set; }
        public SimpleCommand SendRawDataCommand { get; private set; }
        public SimpleCommand CustomCommand { get; private set; }

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

            ProtocolEditorCommand = new SimpleCommand(
                (parameters) => EditProtocol(),
                (parameters) => SerialIsDisconnected);

            CommandEditorCommand = new SimpleCommand(EditCommand);

            CrcCalculatorCommand = new SimpleCommand(StartCrcCalculator);

            SendRawDataCommand = new SimpleCommand((parameters) => SendBytes());

            CustomCommand = new SimpleCommand(Custom);

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
            }
            catch (Exception ex)
            {
                OnError(ex);
            }

            try
            {
                Profiles = AppDataFolderFileHelper.LoadFiles<Profile>();
            }
            catch (Exception ex)
            {
                OnError(ex);
            }

            AddDefaultProtocols();
            AddDefaultProfiles();

            _currentProfile = _defaultProfile;
            _currentProtocol = _defaultProtocol;

            SetSelectedProfileAndProtocolWithId(_appSettings.ProfileId);
        }

        private void Custom(object? parameter)
        {
            if (parameter is NoProtocolCommand command)
            {
                try
                {
                    byte[] bytes = command.GetBytes();

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
                   new PacketModeEndOfPacketSymbol([(byte)'\r', (byte)'\n']));

                Protocols.Add(newProtocol);

                newProtocol = new Protocol(
                   "CR",
                   AppDataFolderFileHelper.GenerateUnusedId(Protocols),
                   new PacketModeEndOfPacketSymbol([(byte)'\r']));

                Protocols.Add(newProtocol);

                newProtocol = new Protocol(
                   "LF",
                   AppDataFolderFileHelper.GenerateUnusedId(Protocols),
                   new PacketModeEndOfPacketSymbol([(byte)'\n']));

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
                PacketModeTimeout.CreateDefault());

                _defaultProtocol.CanNotDelete = true;
                _defaultProtocol.CanNotEditName = true;

                Protocols.Insert(0, _defaultProtocol);
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

            SetupSettingsAccordingToCurrentProfile();
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

        private void SetupSettingsAccordingToCurrentProfile()
        {
            _comPortViewModel.ComPort.Settings = CurrentProfile.ComPortSettings;

            _comPortViewModel.RefreshPortList();
        }

        private void EditProfile()
        {
            List<Profile> profilesCopy = [];

            for (int i = 0; i < Profiles.Count; ++i)
            {
                profilesCopy.Add(Profiles[i].CreateClone());
            }

            ProfileListEditor profileEditor = new ProfileListEditor(
                profilesCopy,
                Profiles.IndexOf(CurrentProfile),
                ShowErrorDialog);

            bool ok = ShowDialog.ShowDialog(
                profileEditor,
                "Profile Editor",
                ResizeMode.CanResize,
                SizeToContent.Manual,
                false);

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
            List<ProtocolEditor> protocolEditorViewModels = [];
            for (int i = 0; i < Protocols.Count; ++i)
            {
                protocolEditorViewModels.Add(new ProtocolEditor(Protocols[i].CreateClone(), ShowErrorDialog));
            }

            ProtocolListEditor protocolListEditor = new ProtocolListEditor(
                protocolEditorViewModels,
                Protocols.IndexOf(CurrentProtocol),
                ShowErrorDialog);

            bool ok = ShowDialog.ShowDialog(
                protocolListEditor,
                "Protocol Editor",
                ResizeMode.CanResize,
                SizeToContent.Manual,
                false,
                900, 600);

            if (ok)
            {
                _defaultProtocol = protocolListEditor.ProtocolViewModels.First((x) => x.Protocol.Id == Guid.Empty).Protocol;

                if (protocolListEditor.SelectedProtocolViewModel != null)
                {
                    CurrentProtocol = protocolListEditor.SelectedProtocolViewModel.Protocol;
                }
                else
                {
                    CurrentProtocol = _defaultProtocol;
                }

                List<Protocol> newProtocols = [];
                for (int i = 0; i < protocolListEditor.ProtocolViewModels.Count; ++i)
                {
                    newProtocols.Add(protocolListEditor.ProtocolViewModels[i].Protocol);
                }

                Protocols = new ObservableCollection<Protocol>(newProtocols);

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

        private void EditCommand(object? parameter)
        {
            int index = -1;
            if (parameter is Command editingCommand)
            {
                index = Commands.IndexOf(editingCommand);
            }

            List<CommandEditor> commandEditorViewModels = [];
            for (int i = 0; i < Commands.Count; ++i)
            {
                commandEditorViewModels.Add(new CommandEditor(Commands[i].CreateClone(), ShowErrorDialog));
            }

            CommandListEditor commandListEditor = new CommandListEditor(
                commandEditorViewModels,
                index,
                ShowErrorDialog);

            bool ok = ShowDialog.ShowDialog(
                commandListEditor,
                "Command Editor",
                ResizeMode.CanResize,
                SizeToContent.Manual,
                false);

            if (ok)
            {
                List<Command> newCommands = [];
                for (int i = 0; i < commandListEditor.CommandViewModels.Count; ++i)
                {
                    newCommands.Add(commandListEditor.CommandViewModels[i].Command);
                }

                CurrentProfile.Commands = new ObservableCollection<Command>(newCommands);
            }
        }

        private void StartCrcCalculator(object? obj)
        {
            SeriAllPort.ViewModels.Tools.CrcCalculator crcCalculator = new Tools.CrcCalculator();

            bool ok = ShowDialog.ShowDialog(
                crcCalculator,
                "Error Detection Code Calculator",
                ResizeMode.CanResize,
                SizeToContent.Manual,
                false);

            if (ok)
            {
            }
        }

        private void Serial_Error(object sender, System.IO.ErrorEventArgs e)
        {
            OnError(e.GetException());
        }

        private void Serial_ConnectionStateChanged(object? sender, CommonWpf.EventHandlers.ConnectionStateChangedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                SerialIsDisconnected = (e.ConnectionState == ConnectionState.Disconnected);

                if (e.ConnectionState == ConnectionState.Connected
                    || e.ConnectionState == ConnectionState.Disconnected)
                {
                    LogViewModel.AppendLog($"Connection State: {e.ConnectionState}");
                }

                if (e.ConnectionState == ConnectionState.Disconnected)
                {
                    CurrentProtocol.PacketMode.Terminate();
                }
            });
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

                        if (eventNow is PacketReceived packetReceived)
                        {
                            sb.Append($"R.Packet: ");

                            int entryCount = 0;
                            if (CurrentProfile.LogDisplayBytes)
                            {
                                ++entryCount;
                            }

                            if (CurrentProfile.LogDisplayText)
                            {
                                ++entryCount;
                            }

                            if (CurrentProfile.LogDisplayParsed)
                            {
                                ++entryCount;
                            }

                            string newLine = string.Empty;
                            bool multipleEntry = false;
                            if (entryCount >= 2)
                            {
                                newLine = "\r\n";
                                multipleEntry = true;
                            }

                            if (CurrentProfile.LogDisplayBytes)
                            {
                                sb.Append($"{newLine}{(multipleEntry ? "Bytes: " : string.Empty)}: {bytes.BytesToString()}");
                            }

                            if (CurrentProfile.LogDisplayText)
                            {
                                string text = Encoding.UTF8.GetString(bytes);
                                string cleaned = RegexVisibleCharacters().Replace(text, "");
                                sb.Append($"{newLine}{(multipleEntry ? "Text: " : string.Empty)}{cleaned}");
                            }

                            if (CurrentProfile.LogDisplayParsed)
                            {
                                sb.Append($"{newLine}Parsed Bytes:");
                                List<ParsePacketField> fields = packetReceived.PacketFields;

                                for (int i = 0; i < fields.Count; ++i)
                                {
                                    ParsePacketField field = fields[i];
                                    if (field.Field is not EndOfPacketSymbol
                                        && field.Field is not Preamble)
                                    {
                                        byte[]? fieldBytes = field.ActualData;
                                        sb.Append($"\r\n\t{field.Name}: {fieldBytes?.BytesToString()}");
                                    }
                                }
                            }
                        }
                        else if (eventNow is NonPacketBytesReceived)
                        {
                            sb.Append($"R.Error : ");
                            sb.Append($"{bytes.BytesToString()}");
                        }

                        LogViewModel.AppendLog(eventNow.Time, $"{sb}");
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
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                LastErrorMessage = exception.Message;
                LogViewModel.AppendLog($"Error: {LastErrorMessage}");
                ShowErrorDialog.ShowError("Error", LastErrorMessage);
            });
        }

        [GeneratedRegex(@"[^\u0020-\u007E]+")]
        private static partial Regex RegexVisibleCharacters();
    }
}

