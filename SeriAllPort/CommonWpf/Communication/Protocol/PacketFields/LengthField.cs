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
            set => base.LengthMode = LengthMode.FixedLength;
        }

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

        public LengthField(
            string name,
            int byteCount,
            int startFieldIndex,
            int endFieldIndex,
            int valueOffset)
             : base(
                   name,
                   LengthMode.FixedLength,
                   null,
                   byteCount)
        {
            StartFieldIndex = startFieldIndex;
            EndFieldIndex = endFieldIndex;
            ValueOffset = valueOffset;
        }

        [JsonConstructor]
        public LengthField(
            string name,
            TextBytesViewModel textBytes,
            int startFieldIndex,
            int endFieldIndex,
            int valueOffset)
             : base(
                   name,
                   LengthMode.FixedLength,
                   textBytes,
                   textBytes.Bytes.Length)
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
