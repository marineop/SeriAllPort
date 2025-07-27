using CommonWpf.Communication.Protocol.PacketFields;
using CommonWpf.ViewModels.TextBytes;
using System.Text.Json.Serialization;

namespace CommonWpf.Communication.Protocol.PacketModes
{
    public class PacketModeLengthField : PacketMode
    {
        [JsonIgnore]
        public override string Name => "Length Field";

        private LengthField? _lengthField = null;

        public PacketModeLengthField()
            : base()
        {
        }

        public static PacketModeLengthField CreateDefault()
        {
            PacketModeLengthField packetModeLengthField = new PacketModeLengthField();

            Preamble preamble = new Preamble(
                "Preamble",
                new TextBytesViewModel(TextRepresentation.Bytes, [0x00]));

            packetModeLengthField.Fields.Add(preamble);

            PacketField type = new PacketField(
               "Type",
               LengthMode.FixedLength,
               new TextBytesViewModel(),
               1);

            packetModeLengthField.Fields.Add(type);

            LengthField lengthField = new LengthField(
                "Length",
                1,
                3, 3, 0);

            packetModeLengthField.Fields.Add(lengthField);

            PacketField data = new PacketField(
               "Data",
               LengthMode.VariableLength,
               new TextBytesViewModel(),
               0);

            packetModeLengthField.Fields.Add(data);

            return packetModeLengthField;
        }

        protected override void ValidateInternal()
        {
            _lengthField = null;

            for (int i = 0; i < Fields.Count; ++i)
            {
                PacketField field = Fields[i];
                if (field is EndOfPacketSymbol)
                {
                    throw new Exception("EOP can not be used in Length Field mode.");
                }
                else if (field is LengthField)
                {
                    _lengthField = field as LengthField;
                }
            }

            if (_lengthField == null)
            {
                throw new Exception("There must be at least 1 Length field.");
            }

            int lengthStartFieldIndex = _lengthField.StartFieldIndex;
            int lengthEndFieldIndex = _lengthField.EndFieldIndex;

            if (lengthStartFieldIndex < 0 || lengthStartFieldIndex > Fields.Count - 1)
            {
                throw new Exception("The start index of the Length field is out of range.");
            }

            if (lengthEndFieldIndex < 0 || lengthEndFieldIndex > Fields.Count - 1)
            {
                throw new Exception("The end index of the Length field is out of range.");
            }

            if (lengthEndFieldIndex < lengthStartFieldIndex)
            {
                throw new Exception("The end index of the Length field must be greater than or equal to the start index.");
            }
        }

        protected override void BytesReceivedInternal(DateTime time)
        {
            ParsePackets(time, false);
        }

        protected override void TerminateInternal()
        {
        }

        protected override PacketMode CreateCloneInternal()
        {
            PacketModeLengthField PacketModeLengthField = new PacketModeLengthField();

            return PacketModeLengthField;
        }

        protected override int ComputePacketLength(ReadOnlySpan<byte> window)
        {
            return 0;
        }
    }
}
