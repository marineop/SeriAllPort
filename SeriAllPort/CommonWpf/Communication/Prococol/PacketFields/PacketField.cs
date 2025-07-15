using System.Text.Json.Serialization;

namespace CommonWpf.Communication.Prococol.PacketFields
{
    [JsonDerivedType(typeof(PacketField), typeDiscriminator: "PacketField")]
    [JsonDerivedType(typeof(EndOfPacketSymbol), typeDiscriminator: "EOP")]
    [JsonDerivedType(typeof(Preamble), typeDiscriminator: "Preamble")]
    public class PacketField : ViewModel
    {
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        private LengthMode _lengthMode = LengthMode.FixedLength;
        public virtual LengthMode LengthMode
        {
            get => _lengthMode;
            set
            {
                if (_lengthMode != value)
                {
                    _lengthMode = value;

                    if (_lengthMode == LengthMode.FixedLength)
                    {
                        Data = Array.Empty<byte>();
                        if (FixedLength <= 1)
                        {
                            FixedLength = 1;
                        }
                    }
                    else if (_lengthMode == LengthMode.FixedData)
                    {
                        if (Data == null || Data.Length < 1)
                        {
                            Data = new byte[1];
                        }
                    }
                    else
                    {
                        FixedLength = 0;
                        Data = Array.Empty<byte>();
                    }

                    OnPropertyChanged();
                }
            }
        }

        private byte[] _data = Array.Empty<byte>();
        public byte[] Data
        {
            get => _data;
            set
            {
                if (_data != value)
                {
                    byte[] newData = value;

                    if (LengthMode == LengthMode.FixedData)
                    {
                        _data = newData ?? throw new Exception("Invalid Data");

                        if (newData.Length <= 0)
                        {
                            newData = new byte[1];
                        }

                        FixedLength = _data.Length;
                    }
                    else
                    {
                        _data = Array.Empty<byte>();
                    }

                    OnPropertyChanged();
                }
            }
        }

        private int _fixedLength = 0;
        public int FixedLength
        {
            get => _fixedLength;
            set
            {
                if (_fixedLength != value)
                {
                    int newValue = value;

                    if (LengthMode == LengthMode.FixedLength)
                    {
                        if (newValue <= 0)
                        {
                            newValue = 1;
                        }
                    }
                    else if (LengthMode == LengthMode.FixedData)
                    {
                        newValue = Data.Length;
                    }
                    else
                    {
                        newValue = 0;
                    }

                    _fixedLength = newValue;
                    OnPropertyChanged();
                }
            }
        }

        private object? _value = null;
        [JsonIgnore]
        public object? Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged();
                }
            }
        }

        [JsonIgnore]
        public virtual string TypeName { get; } = "Field";

        public PacketField(string name, LengthMode lengthMode, byte[] data, int fixedLength)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            LengthMode = lengthMode;
            Data = data ?? throw new ArgumentNullException(nameof(data));
            FixedLength = fixedLength;
        }

        public virtual PacketField CreateClone()
        {
            PacketField newPacketField = new PacketField(
                Name,
                LengthMode,
                Data = (byte[])Data.Clone(),
                FixedLength);

            return newPacketField;
        }

        protected void AssignWithSelfValue(PacketField packetField)
        {
            packetField.Name = Name;
            packetField.Data = (byte[])Data.Clone();
            packetField.FixedLength = FixedLength;
            packetField.Value = Value;
        }
    }
}
