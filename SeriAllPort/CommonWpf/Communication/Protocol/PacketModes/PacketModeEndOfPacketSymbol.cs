using CommonWpf.Communication.Protocol.EventTypes;
using CommonWpf.Communication.Protocol.PacketFields;
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
                [],
                0);

            Fields.Add(data);

            EndOfPacketSymbol eop = new EndOfPacketSymbol(
                "EOP",
                symbol);

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
                        throw new Exception("EOP length must be at least 1.");
                    }

                    ++eopCount;

                    if (eopCount > 1)
                    {
                        throw new Exception("There can be at most 1 End of Packet Symbol.");
                    }

                    eopIndex = i;
                    _eop = field as EndOfPacketSymbol;
                }
                else if (field is Preamble)
                {
                    preambleIndex = i;
                }
            }

            if (eopIndex < 0)
            {
                throw new Exception("There must be 1 End of Packet Symbol.");
            }

            if (preambleIndex > 0 && preambleIndex > eopIndex)
            {
                throw new Exception("The Preamble field must be placed before the EOP field.");
            }

            for (int i = eopIndex + 1; i < Fields.Count; ++i)
            {
                if (Fields[i].LengthMode != LengthMode.FixedLength)
                {
                    throw new Exception("Fields after End of Packet Symbol must be fixed length.");
                }
                else
                {
                    _byteCountAfterEop += Fields[i].FixedLength;
                }
            }
        }

        protected override void BytesReceivedInternal()
        {
            if (_eop == null)
            {
                throw new Exception("End of Packet symbol must not be null.");
            }

            Span<byte> eopPattern = _eop.Data;

            int windowStartIndexNow = 0;
            int parsedLength = 0;

            while (windowStartIndexNow < _receiveBufferLength)
            {
                Span<byte> windowNow = new Span<byte>(_receiveBuffer, windowStartIndexNow, _receiveBufferLength - windowStartIndexNow);

                if (_preamble != null)
                {
                    Span<byte> preamblePattern = _preamble.Data;
                    int preambleIndex = windowNow.IndexOf(preamblePattern);
                    if (preambleIndex < 0)
                    {
                        break;
                    }
                    else
                    {
                        if (preambleIndex > 0)
                        {
                            byte[] nonPacket = windowNow[..preambleIndex].ToArray();
                            NonPacketBytesReceived nonPacketBytesEvent = new NonPacketBytesReceived(nonPacket);
                            EventQueue.Enqueue(nonPacketBytesEvent);

                            parsedLength += preambleIndex;
                            windowStartIndexNow += preambleIndex;
                            windowNow = windowNow[preambleIndex..];
                        }
                    }
                }

                int eopIndex = windowNow.IndexOf(eopPattern);
                if (eopIndex < 0)
                {
                    break;
                }
                else
                {
                    int packetLength = eopIndex + _eop.FixedLength + _byteCountAfterEop;
                    if (windowNow.Length < packetLength)
                    {
                        break;
                    }
                    else
                    {
                        Span<byte> packetBytes = windowNow[..packetLength];

                        // Parse fields
                        bool fieldsValid = true;
                        List<PacketField> parsedFields = [];

                        int indexNow = 0;
                        for (int i = 0; i < Fields.Count; ++i)
                        {
                            PacketField newField = Fields[i].CreateClone();
                            parsedFields.Add(newField);

                            int fieldLength;
                            if (newField.LengthMode == LengthMode.FixedLength)
                            {
                                fieldLength = newField.FixedLength;
                            }
                            else if (newField.LengthMode == LengthMode.FixedData)
                            {
                                fieldLength = newField.Data.Length;
                            }
                            else // VariableLength
                            {
                                if (i >= Fields.Count - 1)
                                {
                                    // last field must not be variable length
                                    fieldsValid = false;
                                    break;
                                }
                                else
                                {
                                    PacketField nextField = Fields[i + 1];
                                    if (nextField == _eop)
                                    {
                                        fieldLength = eopIndex - indexNow;
                                    }
                                    else if (nextField.LengthMode == LengthMode.FixedData)
                                    {
                                        Span<byte> nextFieldData = nextField.Data;
                                        int nextFileRelativeIndex = packetBytes[indexNow..].IndexOf(nextFieldData);
                                        if (nextFileRelativeIndex >= 0)
                                        {
                                            fieldLength = nextFileRelativeIndex;
                                        }
                                        else
                                        {
                                            // next fixed length field is not exist
                                            fieldsValid = false;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        // [variable length] [variable length]
                                        // [variable length] [Fixed length]
                                        fieldsValid = false;
                                        break;
                                    }
                                }
                            }

                            if (indexNow + fieldLength <= packetBytes.Length)
                            {
                                byte[] newData = packetBytes.Slice(indexNow, fieldLength).ToArray();

                                if (newField.LengthMode == LengthMode.FixedData
                                    && !newData.SequenceEqual(Fields[i].Data))
                                {
                                    // fixed data field, data must be equal to expected data
                                    fieldsValid = false;
                                    break;
                                }

                                newField.Value = newData;

                                indexNow += fieldLength;
                            }
                            else
                            {
                                // Fields specify more bytes than actual packet bytes
                                fieldsValid = false;
                                break;
                            }
                        }

                        if (indexNow != packetLength)
                        {
                            // more bytes in packet than fields specified
                            fieldsValid = false;
                        }

                        if (fieldsValid)
                        {
                            PacketReceived packet = new PacketReceived(parsedFields, packetBytes.ToArray());
                            EventQueue.Enqueue(packet);
                        }
                        else
                        {
                            NonPacketBytesReceived nonPacketBytesEvent = new NonPacketBytesReceived(packetBytes.ToArray());
                            EventQueue.Enqueue(nonPacketBytesEvent);
                        }

                        parsedLength += packetLength;
                        windowStartIndexNow += packetLength;
                    }
                }
            }

            if (parsedLength > 0)
            {
                int remainLength = _receiveBufferLength - parsedLength;
                if (remainLength > 0)
                {
                    Buffer.BlockCopy(_receiveBuffer, parsedLength, _receiveBuffer, 0, remainLength);
                }

                _receiveBufferLength = remainLength;
            }

            if (!EventQueue.IsEmpty)
            {
                RaiseEvent();
            }
        }

        protected override void TerminateInternal()
        {
        }

        protected override PacketMode CreateCloneInternal()
        {
            PacketModeEndOfPacketSymbol packetModeEndOfPacketSymbol = new PacketModeEndOfPacketSymbol();

            return packetModeEndOfPacketSymbol;
        }
    }
}
