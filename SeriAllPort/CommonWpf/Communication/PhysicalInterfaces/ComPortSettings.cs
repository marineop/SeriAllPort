using System.ComponentModel;
using System.IO.Ports;

namespace CommonWpf.Communication.PhysicalInterfaces
{
    public class ComPortSettings : INotifyPropertyChanged
    {
        private string _portName = string.Empty;
        public string PortName
        {
            get => _portName;
            set
            {
                if (_portName != value)
                {
                    _portName = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _baudRate = 9600;
        public int BaudRate
        {
            get => _baudRate;
            set
            {
                if (_baudRate != value)
                {
                    _baudRate = value;
                    OnPropertyChanged();
                }
            }
        }

        private Parity _parity = Parity.None;
        public Parity Parity
        {
            get => _parity;
            set
            {
                if (_parity != value)
                {
                    _parity = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _dataBits = 8;
        public int DataBits
        {
            get => _dataBits;
            set
            {
                if (_dataBits != value)
                {
                    _dataBits = value;
                    OnPropertyChanged();
                }
            }
        }

        private StopBits _stopBits = StopBits.One;
        public StopBits StopBits
        {
            get => _stopBits;
            set
            {
                if (_stopBits != value)
                {
                    _stopBits = value;
                    OnPropertyChanged();
                }
            }
        }

        public ComPortSettings Clone()
        {
            ComPortSettings copy = new ComPortSettings();
            copy.PortName = PortName;
            copy.BaudRate = BaudRate;
            copy.Parity = Parity;
            copy.DataBits = DataBits;
            copy.StopBits = StopBits;

            return copy;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
