using CommonWpf.Communication.Protocol.EventTypes;
using CommonWpf.Communication.Protocol.PacketFields;
using CommonWpf.ViewModels.TextBytes;
using System.Text.Json.Serialization;

namespace CommonWpf.Communication.Protocol.PacketModes
{
    public class PacketModeTimeout : PacketMode
    {
        private readonly System.Timers.Timer _timer = new System.Timers.Timer();
        private readonly int dummy;

        [JsonIgnore]
        public override string Name => "Timeout";

        public PacketModeTimeout()
            : base()
        {
            Initialize();
        }

        public PacketModeTimeout(int dummy)
            : base()
        {
            Initialize();

            PacketField packetField = new PacketField(
                "Data",
                LengthMode.VariableLength,
                new TextBytesViewModel(),
                0);

            Fields.Add(packetField);
            this.dummy = dummy;
        }

        private void Initialize()
        {
            IdleTimeoutMs = 50;

            _timer.AutoReset = false;
            _timer.Elapsed += _timer_Elapsed;
        }

        protected override void BytesReceivedInternal(DateTime time)
        {
            _timer.Stop();
            _timer.Start();
        }

        protected override void ValidateInternal()
        {
            _timer.Enabled = true;
            _timer.Interval = IdleTimeoutMs;
            _timer.Enabled = false;

            foreach (PacketField field in Fields)
            {
                if (field is EndOfPacketSymbol)
                {
                    throw new Exception("Timeout mode can not have EOP field.");
                }
            }
        }

        protected override void TerminateInternal()
        {
            _timer.Stop();
        }

        protected override PacketMode CreateCloneInternal()
        {
            PacketModeTimeout newMode = new PacketModeTimeout();

            return newMode;
        }

        private void _timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime time = DateTime.Now;
            lock (_lock)
            {
                Span<byte> windowNow = new Span<byte>(ReceiveBuffer, 0, _receiveBufferLength);

                while (true)
                {
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
                                NonPacketBytesReceived nonPacketBytesEvent = new NonPacketBytesReceived(time, nonPacket);
                                EventQueue.Enqueue(nonPacketBytesEvent);

                                windowNow = windowNow[preambleIndex..];
                            }
                        }
                    }

                    int packetLength = windowNow.Length;
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
                                    fieldLength = packetBytes.Length - indexNow;
                                }
                                else
                                {
                                    PacketField nextField = Fields[i + 1];
                                    if (nextField.LengthMode == LengthMode.FixedData)
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
                                        // two consecutive variable length fields
                                        fieldsValid = false;
                                        break;
                                    }
                                }
                            }

                            if (indexNow + fieldLength <= packetBytes.Length)
                            {
                                newField.Data = packetBytes.Slice(indexNow, fieldLength).ToArray();

                                if (newField.LengthMode == LengthMode.FixedData
                                    && !newField.Data.SequenceEqual(Fields[i].Data))
                                {
                                    // fixed length field data must equal to actual data
                                    fieldsValid = false;
                                    break;
                                }

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
                            PacketReceived packet = new PacketReceived(time, parsedFields, packetBytes.ToArray());
                            EventQueue.Enqueue(packet);
                        }
                        else
                        {
                            NonPacketBytesReceived nonPacketBytesEvent = new NonPacketBytesReceived(time, packetBytes.ToArray());
                            EventQueue.Enqueue(nonPacketBytesEvent);
                        }
                    }

                    break;
                }

                _receiveBufferLength = 0;

                if (!EventQueue.IsEmpty)
                {
                    RaiseEvent();
                }
            }
        }
    }
}
