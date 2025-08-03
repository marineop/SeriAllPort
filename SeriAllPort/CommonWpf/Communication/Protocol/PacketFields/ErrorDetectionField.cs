using CommonWpf.Communication.ErrorDetection;
using CommonWpf.ViewModels.TextBytes;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace CommonWpf.Communication.Protocol.PacketFields
{
    public class ErrorDetectionField : PacketField
    {
        [JsonIgnore]
        public override string TypeName => "Error Detection";

        public override LengthMode LengthMode
        {
            get => base.LengthMode;
            set
            {
                base.LengthMode = LengthMode.FixedLength;
            }
        }

        [JsonIgnore]
        public override bool CanEditLengthMode { get; } = false;

        [JsonIgnore]
        public override TextBytes TextBytes { get => base.TextBytes; set => base.TextBytes = value; }

        [JsonIgnore]
        public override int FixedLength
        {
            get => base.FixedLength;
            set
            {
                int byteCount = ErrorDetection?.ByteCount ?? 0;
                base.FixedLength = byteCount;
            }
        }

        [JsonIgnore]
        public override bool CanEditFixedLength => false;

        private int _startFieldIndex = 0;
        public int StartFieldIndex
        {
            get => _startFieldIndex;
            set
            {
                if (_startFieldIndex != value)
                {
                    _startFieldIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _endFieldIndex = 0;
        public int EndFieldIndex
        {
            get => _endFieldIndex;
            set
            {
                if (_endFieldIndex != value)
                {
                    _endFieldIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        private IErrorDetection? _errorDetection;
        public IErrorDetection? ErrorDetection
        {
            get => _errorDetection;
            set
            {
                if (_errorDetection != value)
                {
                    _errorDetection = value;
                    OnPropertyChanged();

                    UpdateFixedLength();
                }
            }
        }

        private ObservableCollection<IErrorDetection> _errorDetectionMethods;
        [JsonIgnore]
        public ObservableCollection<IErrorDetection> ErrorDetectionMethods
        {
            get => _errorDetectionMethods;
            set
            {
                if (_errorDetectionMethods != value)
                {
                    _errorDetectionMethods = value;
                    OnPropertyChanged();
                }
            }
        }

        public ErrorDetectionField(
           string name,
           int startFieldIndex,
           int endFieldIndex)
            : base(
                  name,
                  LengthMode.FixedLength,
                  new TextBytes(TextRepresentation.Bytes, new byte[2]),
                  2)
        {
            StartFieldIndex = startFieldIndex;
            EndFieldIndex = endFieldIndex;

            List<IErrorDetection> list = ErrorDetectionHelper.GetErrorDetectionMethodList();

            _errorDetectionMethods = new ObservableCollection<IErrorDetection>(list);

            _errorDetection = list.Find((x) => x.Name.Contains("CRC-16/MODBUS"));
        }

        [JsonConstructor]
        public ErrorDetectionField(
            string name,
            int startFieldIndex,
            int endFieldIndex,
            IErrorDetection errorDetection)
             : base(
                   name,
                   LengthMode.FixedLength,
                   new TextBytes(TextRepresentation.Bytes, new byte[2]),
                   2)
        {
            StartFieldIndex = startFieldIndex;
            EndFieldIndex = endFieldIndex;

            List<IErrorDetection> list = ErrorDetectionHelper.GetErrorDetectionMethodList();

            if (errorDetection == null)
            {
                _errorDetection = list.Find((x) => x.Name.Contains("CRC-16/MODBUS"));

                if (_errorDetection == null)
                {
                    _errorDetection = new CRC();
                }
            }
            else
            {
                int index = list.FindIndex((x) => x.Name == errorDetection.Name);
                if (errorDetection is CRC)
                {
                    _errorDetection = list[index];
                }
                else
                {
                    list.RemoveAt(index);
                    list.Insert(index, errorDetection);
                    _errorDetection = errorDetection;
                }
            }

            _errorDetectionMethods = new ObservableCollection<IErrorDetection>(list);

            UpdateFixedLength();
        }

        private void UpdateFixedLength()
        {
            int byteCount = ErrorDetection?.ByteCount ?? 0;

            TextBytes = new TextBytes(TextRepresentation.Bytes, new byte[byteCount]);
            FixedLength = byteCount;
        }

        public override PacketField CreateClone()
        {
            ErrorDetectionField newPacketField = new ErrorDetectionField(Name, StartFieldIndex, EndFieldIndex, ErrorDetection);

            AssignWithSelfValue(newPacketField);

            return newPacketField;
        }

        public override string ToString()
        {
            return $"{base.ToString()}: E.start={StartFieldIndex}, E.end={EndFieldIndex}";
        }
    }
}
