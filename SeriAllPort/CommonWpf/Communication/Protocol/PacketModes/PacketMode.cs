using CommonWpf.Communication.Protocol.EventTypes;
using CommonWpf.Communication.Protocol.PacketFields;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace CommonWpf.Communication.Protocol.PacketModes
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

        private double _idleTimeoutMs = 50;
        public double IdleTimeoutMs
        {
            get => _idleTimeoutMs;
            set
            {
                if (_idleTimeoutMs != value)
                {
                    _idleTimeoutMs = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<PacketField> _fields = [];
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
        public byte[] ReceiveBuffer { protected get; set; } = [];
        protected int _receiveBufferLength = 0;

        public void Validate()
        {
            _preamble = null;
            int preambleCount = 0;

            HashSet<string> names = [];

            if (Fields.Count <= 0)
            {
                throw new Exception("There must be at least 1 Field.");
            }

            PacketField? previousField = null;
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

                if (previousField != null
                    && previousField.LengthMode == LengthMode.VariableLength
                    && field.LengthMode != LengthMode.FixedData)
                {
                    throw new Exception("A variable-length field must be followed by a fixed-Data field.");
                }

                previousField = field;
            }

            ValidateInternal();
        }

        public void BytesReceived()
        {
            lock (_lock)
            {
                if (Serial is null)
                {
                    throw new NotImplementedException("Implementation Error, Serial must not be null");
                }

                _receiveBufferLength += Serial.ReadBytes(ReceiveBuffer, _receiveBufferLength, ReceiveBuffer.Length - _receiveBufferLength);

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
            PacketMode newPacketMode = CreateCloneInternal();

            newPacketMode.PacketReceived = PacketReceived;

            newPacketMode.Serial = Serial;
            newPacketMode.IdleTimeoutMs = IdleTimeoutMs;

            for (int i = 0; i < Fields.Count; ++i)
            {
                newPacketMode.Fields.Add(Fields[i].CreateClone());
            }

            return newPacketMode;
        }

        protected abstract void ValidateInternal();

        protected abstract void BytesReceivedInternal();

        protected abstract void TerminateInternal();

        protected abstract PacketMode CreateCloneInternal();

        protected void EnqueueEvent(PacketEventType eventType)
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
