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

        private bool _isfixedLength = true;
        public virtual bool IsFixedLength
        {
            get => _isfixedLength;
            set
            {
                if (_isfixedLength != value)
                {
                    _isfixedLength = value;

                    if (_isfixedLength)
                    {
                        FixedLength = 1;
                    }
                    else
                    {
                        FixedLength = 0;
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
                    _data = newData ?? throw new Exception("Invalid Data");

                    if (IsFixedLength && _data != null && _data.Length >= 0)
                    {
                        FixedLength = _data.Length;
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

                    if (!IsFixedLength)
                    {
                        newValue = 0;
                    }
                    else
                    {
                        if (newValue <= 0)
                        {
                            newValue = 1;
                        }
                    }

                    _fixedLength = newValue;

                    if (newValue != Data.Length)
                    {
                        byte[] newData = new byte[newValue];
                        Array.Copy(Data, newData, Math.Min(Data.Length, newValue));
                        Data = newData;
                    }

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

        public PacketField(string name, bool isFixedLength, byte[] data, int fixedLength)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            IsFixedLength = isFixedLength;
            Data = data ?? throw new ArgumentNullException(nameof(data));
            FixedLength = fixedLength;
        }

        public virtual PacketField CreateClone()
        {
            PacketField newPacketField = new PacketField(
                Name,
                IsFixedLength,
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
