using CommonWpf;
using CommonWpf.Communication;
using CommonWpf.Communication.ErrorDetection;
using CommonWpf.Extensions;
using CommonWpf.ViewModels.TextBytes;
using System.Collections.ObjectModel;

namespace SeriAllPort.ViewModels.Tools
{
    public class CrcCalculator : ViewModel
    {
        private ObservableCollection<IErrorDetection> _crcList = [];
        public ObservableCollection<IErrorDetection> CrcList
        {
            get => _crcList;
            set
            {
                if (_crcList != value)
                {
                    _crcList = value;
                    OnPropertyChanged();
                }
            }
        }

        private IErrorDetection? _selectedCrc;
        public IErrorDetection? SelectedCrc
        {
            get => _selectedCrc;
            set
            {
                if (_selectedCrc != value)
                {
                    _selectedCrc = value;
                    OnPropertyChanged();

                    UpdateCrc();
                }
            }
        }

        private string _crcValue = string.Empty;
        public string CrcValue
        {
            get => _crcValue;
            set
            {
                if (_crcValue != value)
                {
                    _crcValue = value;
                    OnPropertyChanged();
                }
            }
        }

        private Endianness _endianness = Endianness.LittleEndian;
        public Endianness Endianness
        {
            get => _endianness;
            set
            {
                if (_endianness != value)
                {
                    _endianness = value;
                    OnPropertyChanged();

                    UpdateCrc();
                }
            }
        }

        private TextBytes _inputData = new TextBytes();
        public TextBytes InputData
        {
            get => _inputData;
            set
            {
                if (_inputData != value)
                {
                    _inputData = value;
                    OnPropertyChanged();
                }
            }
        }

        public CrcCalculator()
        {
            InputData.PropertyChanged += InputData_PropertyChanged;

            CrcList = new ObservableCollection<IErrorDetection>(ErrorDetectionHelper.GetErrorDetectionMethodList());

            foreach (IErrorDetection crc in CrcList)
            {
                crc.PropertyChanged += Crc_PropertyChanged;
            }

            _selectedCrc = CrcList[0];

            UpdateCrc();
        }

        private void Crc_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateCrc();
        }

        private void InputData_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(InputData.Bytes))
            {
                UpdateCrc();
            }
        }

        private void UpdateCrc()
        {
            if (SelectedCrc != null)
            {
                byte[] crc = new byte[8];
                int byteCount = SelectedCrc.ComputeErrorDetectionCode(
                    InputData.Bytes,
                    0, InputData.Bytes.Length,
                    crc,
                    Endianness);

                CrcValue = crc.SubArray(0, byteCount).BytesToString();
            }
        }
    }
}
