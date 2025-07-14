using CommonWpf.Communication;
using CommonWpf.EventHandlers;
using System.Collections.ObjectModel;
using System.IO;

namespace CommonWpf.ViewModels
{
    public class SerialViewModel : ViewModel, ISerial
    {
        #region ISerial

        public event ErrorEventHandler? Error;
        public event ConnectionStateChangedEventHandler? ConnectionStateChanged;
        public event EventHandler? BytesReceived;

        public string Name => CurrentInterface?.Name ?? "null";

        private ConnectionState _connectionState = Communication.ConnectionState.Disconnected;
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

        public int ReadBytes(byte[] bytes, int offset, int capacity)
        {
            return CurrentInterface?.ReadBytes(bytes, offset, capacity) ?? 0;
        }

        public void SendBytes(byte[] bytes)
        {
            CurrentInterface?.SendBytes(bytes);
        }

        public void SendBytes(byte[] bytes, int offset, int length)
        {
            CurrentInterface?.SendBytes(bytes, offset, length);
        }

        #endregion

        public SerialViewModel()
        {
        }

        private ISerial? _currentInterface;
        public ISerial? CurrentInterface
        {
            get => _currentInterface;
            set
            {
                if (_currentInterface != value)
                {
                    if (_currentInterface != null)
                    {
                        _currentInterface.Error -= RaiseConnectionStateChangedEvent;
                        _currentInterface.ConnectionStateChanged -= RaiseConnectionStateChangedEvent;
                        _currentInterface.BytesReceived -= RaiseBytesReceivedEvent;
                    }

                    _currentInterface = value;

                    if (_currentInterface != null)
                    {
                        _currentInterface.Error += RaiseConnectionStateChangedEvent;
                        _currentInterface.ConnectionStateChanged += RaiseConnectionStateChangedEvent;
                        _currentInterface.BytesReceived += RaiseBytesReceivedEvent;

                        ConnectionState = _currentInterface.ConnectionState;
                    }

                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<ISerial> _physicalInterfaces = new ObservableCollection<ISerial>();
        public ObservableCollection<ISerial> PhysicalInterfaces
        {
            get => _physicalInterfaces;
            private set
            {
                if (_physicalInterfaces != value)
                {
                    _physicalInterfaces = value;
                    OnPropertyChanged();
                }
            }
        }

        private void RaiseConnectionStateChangedEvent(object sender, ErrorEventArgs e)
        {
            Error?.Invoke(this, e);
        }

        private void RaiseConnectionStateChangedEvent(object? sender, ConnectionStateChangedEventArgs e)
        {
            ConnectionState = e.ConnectionState;
            ConnectionStateChanged?.Invoke(this, e);
        }

        private void RaiseBytesReceivedEvent(object? sender, EventArgs e)
        {
            BytesReceived?.Invoke(this, e);
        }
    }
}
