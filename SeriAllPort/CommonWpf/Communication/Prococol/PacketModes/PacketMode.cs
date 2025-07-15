using CommonWpf.Communication.Prococol.EventTypes;
using CommonWpf.Communication.Prococol.PacketFields;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace CommonWpf.Communication.Prococol.PacketModes
{
    [JsonDerivedType(typeof(PacketModeEndOfPacketSymbol), typeDiscriminator: "EOP")]
    [JsonDerivedType(typeof(PacketModeTimeout), typeDiscriminator: "Timeout")]
    public abstract class PacketMode : ViewModel
    {
        public event EventHandler? PacketReceived;

        private ConcurrentQueue<PacketEventType> _eventQueue = new ConcurrentQueue<PacketEventType>();
        [JsonIgnore]
        public ConcurrentQueue<PacketEventType> EventQueue
        {
            get => _eventQueue;
            set
            {
                if (_eventQueue != value)
                {
                    _eventQueue = value;
                }
            }
        }

        private ISerial? _serial;
        [JsonIgnore]
        public ISerial? Serial
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

        private double _timeoutMs = 50;
        public double TimeoutMs
        {
            get => _timeoutMs;
            set
            {
                if (_timeoutMs != value)
                {
                    _timeoutMs = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<PacketField> _fields = new ObservableCollection<PacketField>();
        public ObservableCollection<PacketField> Fields
        {
            get => _fields;
            set
            {
                if (_fields != value)
                {
                    _fields = value;
                    OnPropertyChanged();
                }
            }
        }

        [JsonIgnore]
        public abstract string Name { get; }

        protected PacketField? _preamble;

        protected object _lock = new object();
        protected byte[] _receiveBuffer = new byte[4096];
        protected int _receiveBufferLength = 0;

        public void Validate()
        {
            _preamble = null;
            int preambleCount = 0;

            HashSet<string> names = new HashSet<string>();

            if (Fields.Count <= 0)
            {
                throw new Exception("There must be at least 1 Field.");
            }

            PacketField? previousfield = null;
            for (int i = 0; i < Fields.Count; ++i)
            {
                PacketField field = Fields[i];
                if (field is Preamble)
                {
                    if (i != 0)
                    {
                        throw new Exception("Preamble must be first field.");
                    }

                    ++preambleCount;

                    if (preambleCount > 1)
                    {
                        throw new Exception("There can be only 1 Preamble.");
                    }

                    _preamble = field;
                }

                if (names.Contains(field.Name))
                {
                    throw new Exception($"Field name must be unique: {field.Name}");
                }
                else
                {
                    names.Add(field.Name);
                }

                if (field.LengthMode == LengthMode.FixedLength && field.FixedLength <= 0)
                {
                    throw new Exception("Fixed Length field's length must be at least 1.");
                }

                if (field.LengthMode == LengthMode.FixedData && field.Data.Length <= 0)
                {
                    throw new Exception("Fixed Data field's Data length must be at least 1.");
                }

                if (previousfield != null
                    && previousfield.LengthMode == LengthMode.VariableLength
                    && field.LengthMode != LengthMode.FixedData)
                {
                    throw new Exception("A variable-length field must be followed by a fixed-Data field.");
                }

                previousfield = field;
            }

            ValidateInternal();
        }

        public void BytesReceived()
        {
            lock (_lock)
            {
                _receiveBufferLength += Serial.ReadBytes(_receiveBuffer, _receiveBufferLength, _receiveBuffer.Length - _receiveBufferLength);

                BytesReceivedInternal();
            }
        }

        public void Terminate()
        {
            lock (_lock)
            {
                TerminateInternal();

                _receiveBufferLength = 0;
            }
        }

        public PacketMode CreateClone()
        {
            PacketMode newPacketMode = CreateCloneInteranl();

            newPacketMode.PacketReceived = PacketReceived;

            newPacketMode.Serial = Serial;
            newPacketMode.TimeoutMs = TimeoutMs;

            for (int i = 0; i < Fields.Count; ++i)
            {
                newPacketMode.Fields.Add(Fields[i].CreateClone());
            }

            return newPacketMode;
        }

        protected abstract void ValidateInternal();

        protected abstract void BytesReceivedInternal();

        protected abstract void TerminateInternal();

        protected abstract PacketMode CreateCloneInteranl();

        protected void EnqueuEvent(PacketEventType eventType)
        {
            EventQueue.Enqueue(eventType);
        }

        protected void RaiseEvent()
        {
            Task.Run(() =>
            {
                PacketReceived?.Invoke(this, EventArgs.Empty);
            });
        }
    }
}
