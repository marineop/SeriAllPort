using CommonWpf.Communication;

namespace CommonWpf.ViewModels
{
    public class SerialViewModel : ViewModel
    {
        private ISerial _currentInterface;
        public ISerial CurrentInterface
        {
            get => _currentInterface;
            set
            {
                if (_currentInterface != value)
                {
                    _currentInterface = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<ISerial> _physicalInterfaces;
        public List<ISerial> PhysicalInterfaces
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

        private ComPortViewModel _comPortViewModel;
        public ComPortViewModel ComPortViewModel
        {
            get => _comPortViewModel;
            set
            {
                if (_comPortViewModel != value)
                {
                    _comPortViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        public SerialViewModel()
        {
            _comPortViewModel = new ComPortViewModel();

            _physicalInterfaces = new List<ISerial>()
            {
                _comPortViewModel
            };

            _currentInterface = _physicalInterfaces[0];
        }
    }
}
