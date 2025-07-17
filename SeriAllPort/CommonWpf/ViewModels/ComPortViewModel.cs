using CommonWpf.Communication;
using CommonWpf.Communication.PhysicalInterfaces;
using CommonWpf.EventHandlers;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Ports;
using System.Windows.Input;

namespace CommonWpf.ViewModels
{
    public class ComPortViewModel : ViewModel, ISerial
    {
        public event ErrorEventHandler Error
        {
            add
            {
                _comPort.Error += value;
            }
            remove
            {
                _comPort.Error -= value;
            }
        }
        public event ConnectionStateChangedEventHandler ConnectionStateChanged
        {
            add
            {
                _comPort.ConnectionStateChanged += value;
            }
            remove
            {
                _comPort.ConnectionStateChanged -= value;
            }
        }
        public event EventHandler? BytesReceived
        {
            add
            {
                _comPort.BytesReceived += value;
            }
            remove
            {
                _comPort.BytesReceived -= value;
            }
        }

        private ComPort _comPort = new ComPort();
        public ComPort ComPort
        {
            get => _comPort;
            private set
            {
                if (_comPort != value)
                {
                    _comPort = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name => _comPort.Name;

        private ConnectionState _connectionState;
        public ConnectionState ConnectionState
        {
            get => _connectionState;
            set
            {
                if (_connectionState != value)
                {
                    _connectionState = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<string> _portNameList = [];
        public ObservableCollection<string> PortNameList
        {
            get => _portNameList;
            set
            {
                if (_portNameList != value)
                {
                    _portNameList = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<int> BaudRateList { get; private set; } =
        [
            300,
            600,
            1200,
            1800,
            2400,
            4800,
            9600,
            19200,
            28800,
            38400,
            57600,
            76800,
            115200,
            230400,
            460800,
            576000,
            921600
        ];
        public List<int> DataBitsList { get; private set; } = [5, 6, 7, 8];

        public IShowDialog? ShowDialog { get; set; }

        public SimpleCommand RefreshPortListCommand { get; set; }

        public SimpleCommand SettingsCommand { get; set; }

        public SimpleCommand ConnectCommand { get; set; }

        public ComPortViewModel()
        {
            RefreshPortListCommand = new SimpleCommand((parameter) => RefreshPortList());
            SettingsCommand = new SimpleCommand((parameter) => OpenDetailedSettingsWindow());
            ConnectCommand = new SimpleCommand((parameter) => Connect());

            ComPort.ConnectionStateChanged += (o, c) => { ConnectionState = c.ConnectionState; };
        }

        public void Connect()
        {
            if (ComPort.ConnectionState == ConnectionState.Disconnected)
            {
                ComPort.Connect();
            }
            else if (ComPort.ConnectionState == ConnectionState.Connected)
            {
                ComPort.Disconnect();
            }
        }

        public int ReadBytes(byte[] bytes, int offset, int capacity)
        {
            return ComPort.ReadBytes(bytes, offset, capacity);
        }

        public void SendBytes(byte[] bytes)
        {
            _comPort.SendBytes(bytes);
        }

        public void SendBytes(byte[] bytes, int offset, int length)
        {
            _comPort.SendBytes(bytes, offset, length);
        }

        public void RefreshPortList()
        {
            PortNameList = new ObservableCollection<string>(SerialPort.GetPortNames());
            if(PortNameList.Count > 0 && string.IsNullOrEmpty(ComPort.Settings.PortName))
            {
                ComPort.Settings.PortName = PortNameList[0];
            }
        }

        public void TrySetPortName(string portName)
        {
            if (PortNameList.Contains(portName))
            {
                ComPort.Settings.PortName = portName;
            }
        }

        public void OpenDetailedSettingsWindow()
        {
            if (ShowDialog != null)
            {
                ComPortViewModel newInstance = new ComPortViewModel();
                newInstance.ComPort.Settings = ComPort.Settings.Clone();
                bool ok = ShowDialog.ShowDialog(newInstance, "Serial Port Settings");
                if (ok)
                {
                    ComPort.Settings = newInstance.ComPort.Settings;
                }
            }
            else
            {
                throw new Exception("ShowDialog not configured.");
            }
        }
    }
}
