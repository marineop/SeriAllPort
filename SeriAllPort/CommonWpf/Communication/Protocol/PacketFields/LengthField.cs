using CommonWpf.ViewModels.TextBytes;
using System.Text.Json.Serialization;

namespace CommonWpf.Communication.Protocol.PacketFields
{
    public class LengthField : PacketField
    {
        [JsonIgnore]
        public override string TypeName => "LengthField";

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

        private int _ValueOffset;
        public int ValueOffset
        {
            get => _ValueOffset;
            set
            {
                if (_ValueOffset != value)
                {
                    _ValueOffset = value;
                    OnPropertyChanged();
                }
            }
        }

        [JsonConstructor]
        public LengthField(
            string name,
            int fixedLength,
            int startFieldIndex,
            int endFieldIndex,
            int valueOffset)
             : base(
                   name,
                   LengthMode.FixedLength,
                   new TextBytes(TextRepresentation.Bytes, new byte[fixedLength]),
                   fixedLength)
        {
            StartFieldIndex = startFieldIndex;
            EndFieldIndex = endFieldIndex;
            ValueOffset = valueOffset;
        }

        public override PacketField CreateClone()
        {
            LengthField newPacketField = new LengthField(Name, KnownLength, StartFieldIndex, EndFieldIndex, ValueOffset);

            AssignWithSelfValue(newPacketField);

            return newPacketField;
        }

        public override string ToString()
        {
            return $"{base.ToString()}: L.start={StartFieldIndex}, L.end={EndFieldIndex}";
        }
    }
}
