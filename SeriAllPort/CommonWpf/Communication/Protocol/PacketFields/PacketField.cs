using CommonWpf.ViewModels.TextBytes;
using System.Text.Json.Serialization;

namespace CommonWpf.Communication.Protocol.PacketFields
{
    [JsonDerivedType(typeof(PacketField), typeDiscriminator: "PacketField")]
    [JsonDerivedType(typeof(EndOfPacketSymbol), typeDiscriminator: "EOP")]
    [JsonDerivedType(typeof(Preamble), typeDiscriminator: "Preamble")]
    [JsonDerivedType(typeof(LengthField), typeDiscriminator: "LengthField")]
    public class  PacketField : ViewModel
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

                    byte[] newBytes = TextBytes.Bytes;

                    if (_lengthMode == LengthMode.FixedLength)
                    {
                        newBytes = [];
                        if (FixedLength <= 1)
                        {
                            FixedLength = 1;
                        }

                        CanEditTextBytes = false;
                    }
                    else if (_lengthMode == LengthMode.FixedData)
                    {
                        if (newBytes == null || newBytes.Length < 1)
                        {
                            newBytes = new byte[1];
                        }

                        CanEditTextBytes = true;
                    }
                    else
                    {
                        FixedLength = 0;
                        newBytes = [];

                        CanEditTextBytes = false;
                    }

                    TextBytes.Bytes = newBytes;
                    TextBytes.SetTextWithCurrentBytes();
                }

                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public virtual bool CanEditLengthMode { get; } = true;

        private TextBytes _textBytes = new();
        public virtual TextBytes TextBytes
        {
            get => _textBytes;
            set
            {
                if (_textBytes != value)
                {
                    _textBytes = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _CanEditTextBytes = true;
        [JsonIgnore]
        public bool CanEditTextBytes
        {
            get => _CanEditTextBytes;
            set
            {
                if (_CanEditTextBytes != value)
                {
                    _CanEditTextBytes = value;
                    OnPropertyChanged();
                }
            }
        }

        [JsonIgnore]
        public byte[] Data
        {
            get => TextBytes.Bytes;
            set => TextBytes.Bytes = value;
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
                        newValue = TextBytes.Bytes.Length;
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

        [JsonIgnore]
        public int KnownLength
        {
            get
            {
                if (LengthMode == LengthMode.FixedLength)
                {
                    return FixedLength;
                }
                else if (LengthMode == LengthMode.FixedData)
                {
                    return Data.Length;
                }
                else
                {
                    return -1;
                }
            }
        }

        [JsonIgnore]
        internal bool CoveredByLengthField { get; set; }

        [JsonIgnore]
        public virtual string TypeName { get; } = "Field";

        private int _index;
        [JsonIgnore]
        public int Index
        {
            get => _index;
            set
            {
                if (_index != value)
                {
                    _index = value;
                    OnPropertyChanged();
                }
            }
        }

        public PacketField(string name, LengthMode lengthMode, TextBytes? textBytes, int fixedLength)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            LengthMode = lengthMode;

            if (LengthMode == LengthMode.FixedLength)
            {
                FixedLength = fixedLength;
                if (textBytes == null)
                {
                    TextBytes.Bytes = [];
                }
                else
                {
                    TextBytes = textBytes;
                }

                CanEditTextBytes = false;
            }
            else if (LengthMode == LengthMode.FixedData)
            {
                if (textBytes == null)
                {
                    throw new Exception("Fixed-Data field must specify textBytes.");
                }

                TextBytes = textBytes;
                FixedLength = TextBytes.Bytes.Length;

                CanEditTextBytes = true;
            }
            else if (LengthMode == LengthMode.VariableLength)
            {
                FixedLength = 0;
                TextBytes.Bytes = [];
                CanEditTextBytes = false;
            }

            TextBytes.SetTextWithCurrentBytes();
            TextBytes.PreviewUpdateBytesHook += UpdateBytesCheck;
            TextBytes.PostUpdateBytesHook += BytesUpdated;
        }

        public static PacketField CreateFixedLength(string name, int fixedLength)
        {
            TextBytes textBytes = new TextBytes(TextRepresentation.Bytes, new byte[fixedLength]);
            PacketField packetField = new PacketField(name, LengthMode.FixedLength, textBytes, fixedLength);

            return packetField;
        }

        public static PacketField CreateFixedData(string name, byte[] fixedData)
        {
            TextBytes textBytes = new TextBytes(TextRepresentation.Bytes, fixedData);
            PacketField packetField = new PacketField(name, LengthMode.FixedData, textBytes, fixedData.Length);

            return packetField;
        }

        public static PacketField CreateVariableLength(string name)
        {
            PacketField packetField = new PacketField(name, LengthMode.VariableLength, new TextBytes(), 0);

            return packetField;
        }

        public virtual PacketField CreateClone()
        {
            TextBytes newTextBytes = TextBytes.CreateClone();
            newTextBytes.SetTextWithCurrentBytes();

            PacketField newPacketField = new PacketField(
                Name,
                LengthMode,
                newTextBytes,
                FixedLength);

            newPacketField.CoveredByLengthField = CoveredByLengthField;

            return newPacketField;
        }

        protected void AssignWithSelfValue(PacketField packetField)
        {
            packetField.Name = Name;

            packetField.TextBytes = TextBytes.CreateClone();
            packetField.TextBytes.SetTextWithCurrentBytes();
            packetField.TextBytes.PreviewUpdateBytesHook += UpdateBytesCheck;
            packetField.TextBytes.PostUpdateBytesHook += BytesUpdated;

            packetField.FixedLength = FixedLength;
        }

        internal void RefreshValues()
        {
            TextBytes.SetTextWithCurrentBytes();
            OnPropertyChanged(nameof(FixedLength));
        }

        private byte[] UpdateBytesCheck(byte[] newData)
        {
            byte[] answer = [];

            if (LengthMode == LengthMode.FixedData)
            {
                if (newData == null)
                {
                    throw new Exception("Invalid Data");
                }

                if (newData.Length <= 0)
                {
                    newData = new byte[1];
                }

                answer = newData;
            }

            return answer;
        }

        private void BytesUpdated()
        {
            FixedLength = TextBytes.Bytes.Length;
        }
    }
}
