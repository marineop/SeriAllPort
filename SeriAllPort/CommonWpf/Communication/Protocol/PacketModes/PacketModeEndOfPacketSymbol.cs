using CommonWpf.Communication.Protocol.PacketFields;
using CommonWpf.ViewModels.TextBytes;
using System.Text.Json.Serialization;

namespace CommonWpf.Communication.Protocol.PacketModes
{
    public class PacketModeEndOfPacketSymbol : PacketMode
    {
        private EndOfPacketSymbol? _eop;
        private int _byteCountAfterEop;

        [JsonIgnore]
        public override string Name => "End of Packet Symbol";

        public PacketModeEndOfPacketSymbol()
            : base()
        {
        }

        public PacketModeEndOfPacketSymbol(byte[] symbol)
             : base()
        {
            PacketField data = new PacketField(
                "Data",
                LengthMode.VariableLength,
                new TextBytesViewModel(),
                0);

            Fields.Add(data);

            EndOfPacketSymbol eop = new EndOfPacketSymbol(
                "EOP",
                new TextBytesViewModel(TextRepresentation.Bytes, symbol));

            Fields.Add(eop);
        }

        protected override void ValidateInternal()
        {
            int eopCount = 0;

            _byteCountAfterEop = 0;
            int eopIndex = -1;
            int preambleIndex = -1;

            for (int i = 0; i < Fields.Count; ++i)
            {
                PacketField field = Fields[i];
                if (field is EndOfPacketSymbol)
                {
                    if (field.LengthMode != LengthMode.FixedData)
                    {
                        throw new Exception("EOP must be fixed data.");
                    }

                    if (field.FixedLength <= 0)
                    {
                        throw new Exception("The length of EOP field must be at least 1.");
                    }

                    ++eopCount;

                    if (eopCount > 1)
                    {
                        throw new Exception("There can be at most 1 EOP.");
                    }

                    eopIndex = i;
                    _eop = field as EndOfPacketSymbol;
                }
                else if (field is Preamble)
                {
                    preambleIndex = i;
                }

                field.CoveredByLengthField = true;
            }

            if (_eop == null)
            {
                throw new Exception("There must be 1 End of Packet Symbol.");
            }

            for (int i = 0; i < Fields.Count; ++i)
            {
                PacketField field = Fields[i];
                if (field != _eop
                    && field.LengthMode == LengthMode.FixedData
                    && _eop.Data.SequenceEqual(field.Data))
                {
                    throw new Exception("The data of Fixed-Data field cannot be the same as the EOP.");
                }
            }

            if (preambleIndex > 0 && preambleIndex > eopIndex)
            {
                throw new Exception("The Preamble field must be placed before the EOP field.");
            }

            for (int i = eopIndex + 1; i < Fields.Count; ++i)
            {
                if (Fields[i].LengthMode != LengthMode.FixedLength)
                {
                    _byteCountAfterEop += Fields[i].FixedLength;
                }
                else if (Fields[i].LengthMode != LengthMode.FixedLength)
                {
                    _byteCountAfterEop += Fields[i].Data.Length;
                }
                else
                {
                    throw new Exception("Fields after End of Packet Symbol must be Fixed-Length or Fixed-Data.");
                }
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
            PacketModeEndOfPacketSymbol packetModeEndOfPacketSymbol = new PacketModeEndOfPacketSymbol();

            return packetModeEndOfPacketSymbol;
        }

        protected override int ComputePacketLength(ReadOnlySpan<byte> window)
        {
            if (_eop == null)
            {
                throw new Exception("Invalid Protocol");
            }

            int index = window.IndexOf(_eop.Data);
            if (index >= 0)
            {
                index += _eop.Data.Length + _byteCountAfterEop;
            }

            return index;
        }
    }
}
