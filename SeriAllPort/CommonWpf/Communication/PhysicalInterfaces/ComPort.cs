using CommonWpf.EventHandlers;
using System.IO;
using System.IO.Ports;

namespace CommonWpf.Communication.PhysicalInterfaces
{
    public class ComPort : ViewModel, ISerial
    {
        public event ErrorEventHandler? Error;
        public event ConnectionStateChangedEventHandler? ConnectionStateChanged;
        public event EventHandler? BytesReceived;

        private readonly SerialPort _serialPort = new SerialPort();
        private readonly object _serialPortReceiveLock = new object();

        private ConnectionState _connectionState;
        public ConnectionState ConnectionState
        {
            get => _connectionState;
            private set
            {
                if (_connectionState != value)
                {
                    _connectionState = value;
                    ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(_connectionState));
                    OnPropertyChanged();
                }
            }
        }

        private ComPortSettings _settings = new ComPortSettings();
        public ComPortSettings Settings
        {
            get => _settings;
            set
            {
                if (_settings != value)
                {
                    _settings = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name => "COM Port";

        public ComPort()
        {
            _serialPort.DataReceived += _serialPort_DataReceived;

            Task.Factory.StartNew(() => 
            {
                while (true)
                {
                    if (ConnectionState == ConnectionState.Connected)
                    {
                        if (!_serialPort.IsOpen)
                        {
                            ConnectionState = ConnectionState.Disconnected;
                        }
                    }
                    Thread.Sleep(1000);
                }
            }, TaskCreationOptions.LongRunning);
        }

        public void Connect()
        {
            try
            {
                _serialPort.PortName = Settings.PortName;
                _serialPort.BaudRate = Settings.BaudRate;
                _serialPort.Parity = Settings.Parity;
                _serialPort.DataBits = Settings.DataBits;
                _serialPort.StopBits = Settings.StopBits;

                ConnectionState = ConnectionState.Connecting;
                _serialPort.Open();
            }
            catch (Exception ex)
            {
                OnError(ex);
            }

            if (_serialPort.IsOpen)
            {
                ConnectionState = ConnectionState.Connected;
            }
            else
            {
                ConnectionState = ConnectionState.Disconnected;
            }
        }

        public void Disconnect()
        {
            try
            {
                _serialPort.Close();
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
            finally
            {
                ConnectionState = ConnectionState.Disconnected;
            }
        }

        public void SendBytes(byte[] bytes)
        {
            if (bytes.Length > 0)
            {
                _serialPort.Write(bytes, 0, bytes.Length);
            }
        }

        public void SendBytes(byte[] bytes, int offset, int length)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(offset);

            ArgumentOutOfRangeException.ThrowIfNegative(length);

            if (offset + length >= bytes.Length)
            {
                throw new IndexOutOfRangeException("(offset + length) must be less than bytes.Length");
            }

            if (length > 0)
            {
                _serialPort.Write(bytes, offset, length);
            }
        }

        public int ReadBytes(byte[] bytes, int offset, int capacity)
        {
            int count = 0;
            lock (_serialPortReceiveLock)
            {
                count = _serialPort.Read(bytes, offset, capacity - offset);
            }

            return count;
        }

        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            BytesReceived?.Invoke(this, EventArgs.Empty);
        }

        private void OnError(Exception exception)
        {
            Error?.Invoke(this, new ErrorEventArgs(exception));
        }
    }
}
