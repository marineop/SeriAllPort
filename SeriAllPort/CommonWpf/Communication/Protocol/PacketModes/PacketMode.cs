using CommonWpf.Communication.Protocol.EventTypes;
using CommonWpf.Communication.Protocol.PacketFields;
using CommonWpf.Communication.Protocol.ParseData;
using CommonWpf.Extensions;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace CommonWpf.Communication.Protocol.PacketModes
{
    [JsonDerivedType(typeof(PacketModeEndOfPacketSymbol), typeDiscriminator: "EOP")]
    [JsonDerivedType(typeof(PacketModeTimeout), typeDiscriminator: "Timeout")]
    [JsonDerivedType(typeof(PacketModeLengthField), typeDiscriminator: "Length")]
    public abstract class PacketMode : ViewModel
    {
        public event EventHandler? DataReceived;

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
                    _fields.CollectionChanged -= _fields_CollectionChanged;

                    _fields = value;

                    _fields.CollectionChanged += _fields_CollectionChanged;

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

        private readonly System.Timers.Timer _timer = new System.Timers.Timer();

        private List<ParsePacketField>? _parsedFields = null;
        private readonly ParseResultData _parseResultData = new();
        private int _parseWindowStart = 0;

        public PacketMode()
        {
            _timer.AutoReset = false;
            _timer.Elapsed += _timer_Elapsed;

            _fields.CollectionChanged += _fields_CollectionChanged;
        }

        public PacketMode CreateClone()
        {
            PacketMode newPacketMode = CreateCloneInternal();

            newPacketMode.DataReceived = DataReceived;

            newPacketMode.Serial = Serial;
            newPacketMode.IdleTimeoutMs = IdleTimeoutMs;

            for (int i = 0; i < Fields.Count; ++i)
            {
                newPacketMode.Fields.Add(Fields[i].CreateClone());
            }

            return newPacketMode;
        }

        public void Validate()
        {
            _preamble = null;
            int preambleCount = 0;

            HashSet<string> names = [];

            if (Fields.Count <= 0)
            {
                throw new Exception("There must be at least 1 Field.");
            }

            for (int i = 0; i < Fields.Count; ++i)
            {
                PacketField field = Fields[i];

                field.RefreshValues();
            }

            for (int i = 0; i < Fields.Count; ++i)
            {
                PacketField field = Fields[i];

                if (field is Preamble)
                {
                    if (i != 0)
                    {
                        throw new Exception("Preamble must be the first field.");
                    }

                    ++preambleCount;

                    if (preambleCount > 1)
                    {
                        throw new Exception("There can be at most 1 Preamble.");
                    }

                    _preamble = field;
                }
            }

            for (int i = 0; i < Fields.Count; ++i)
            {
                PacketField field = Fields[i];

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
                    throw new Exception("The length of Fixed-Length field must be at least 1.");
                }

                if (field.LengthMode == LengthMode.FixedData)
                {
                    if (field.Data.Length <= 0)
                    {
                        throw new Exception("The length of Fixed-Data field must be at least 1.");
                    }

                    //if (_preamble != null)
                    //{
                    //    if (_preamble.Data.SequenceEqual(field.Data))
                    //    {
                    //        throw new Exception("The data of Fixed-Data field cannot be the same as the Preamble.");
                    //    }
                    //}
                }

                if (field is LengthField lengthField)
                {
                    for (int lengthFieldCoverIndex = lengthField.StartFieldIndex; lengthFieldCoverIndex <= lengthField.EndFieldIndex; ++lengthFieldCoverIndex)
                    {
                        Fields[lengthFieldCoverIndex].CoveredByLengthField = true;
                    }
                }
            }

            ValidateInternal();

            TestSimulatedPacket();

            if (IdleTimeoutMs > 0)
            {
                _timer.Enabled = true;
                _timer.Interval = IdleTimeoutMs;
                _timer.Enabled = false;
            }
        }

        public void BytesReceived()
        {
            if (IdleTimeoutMs > 0)
            {
                _timer.Stop();
                _timer.Start();
            }

            lock (_lock)
            {
                if (Serial is null)
                {
                    throw new NotImplementedException("Implementation Error, Serial must not be null");
                }

                DateTime now = DateTime.Now;

                _receiveBufferLength += Serial.ReadBytes(ReceiveBuffer, _receiveBufferLength, ReceiveBuffer.Length - _receiveBufferLength);

                BytesReceivedInternal(now);
            }
        }

        public void ParsePackets(DateTime time, bool isTimeout)
        {
            lock (_lock)
            {
                int lastPacketTail = 0;

                while (_parseWindowStart < _receiveBufferLength)
                {
                    // avoid CreateClone too many times
                    if (_parsedFields == null)
                    {
                        _parsedFields = [];
                        for (int i = 0; i < Fields.Count; ++i)
                        {
                            PacketField newField = Fields[i].CreateClone();
                            _parsedFields.Add(new ParsePacketField(newField));
                        }
                    }
                    else
                    {
                        foreach (ParsePacketField packetField in _parsedFields)
                        {
                            packetField.ResetResolveData();
                        }
                    }

                    // setup current window
                    ReadOnlySpan<byte> window = new ReadOnlySpan<byte>(
                          ReceiveBuffer,
                          _parseWindowStart,
                          _receiveBufferLength - _parseWindowStart);

                    // search preamble
                    if (_preamble != null)
                    {
                        int preambleIndex = window.IndexOf(_preamble.Data);
                        if (preambleIndex < 0)
                        {
                            // bytes before sub preamble can be discarded
                            int subPreambleLength;
                            for (subPreambleLength = _preamble.Data.Length - 1; subPreambleLength >= 1; --subPreambleLength)
                            {
                                if (window[^subPreambleLength..].IndexOf(_preamble.Data.AsSpan()[..subPreambleLength]) >= 0)
                                {
                                    break;
                                }
                            }

                            _parseWindowStart += window.Length - subPreambleLength;

                            break;
                        }
                        else
                        {
                            _parseWindowStart += preambleIndex;

                            window = window[preambleIndex..];
                        }
                    }

                    // parse packet
                    ParseResult parseResult = ParsePacket(window, _parsedFields, _parseResultData);

                    if (parseResult == ParseResult.FullPacket)
                    {
                        // bytes before a valid packet are non-packet bytes
                        if (_parseWindowStart - lastPacketTail > 0)
                        {
                            EnqueueNonPacketBytesEvent(time, ReceiveBuffer[lastPacketTail.._parseWindowStart]);
                        }

                        EnqueuePacketEvent(time, _parsedFields, window[0.._parseResultData.PacketLength].ToArray());

                        _parseWindowStart += _parseResultData.PacketLength;
                        lastPacketTail = _parseWindowStart;

                        _parsedFields = null;
                    }
                    else if (parseResult == ParseResult.WaitForFullPacket)
                    {
                        break;
                    }
                    else if (parseResult == ParseResult.ErrorPacket)
                    {
                        if (_parseResultData.ParseErrorType == ParseErrorType.EopAndFieldsConflict)
                        {
                            EnqueueNonPacketBytesEvent(time, ReceiveBuffer[lastPacketTail..(_parseWindowStart + _parseResultData.PacketLength)]);
                            _parseWindowStart += _parseResultData.PacketLength;
                            lastPacketTail = _parseWindowStart;
                        }
                        else
                        {
                            ++_parseWindowStart;
                        }
                    }
                    else
                    {
                        throw new Exception("Error");
                    }
                }

                // move remain bytes to the front of _receiveBufferLength
                int remainStartIndex = 0;
                if (_preamble != null)
                {
                    // bytes before preamble are non-packet bytes
                    if (_parseWindowStart - lastPacketTail > 0)
                    {
                        EnqueueNonPacketBytesEvent(time, ReceiveBuffer[lastPacketTail.._parseWindowStart]);
                    }

                    remainStartIndex = _parseWindowStart;
                }
                else
                {
                    remainStartIndex = lastPacketTail;
                }

                if (remainStartIndex > 0)
                {
                    int remainLength = _receiveBufferLength - remainStartIndex;
                    if (remainLength > 0)
                    {
                        Buffer.BlockCopy(ReceiveBuffer, remainStartIndex, ReceiveBuffer, 0, remainLength);
                    }

                    _receiveBufferLength = remainLength;

                    _parseWindowStart -= remainStartIndex;
                }

                // if timeout than empty the ReceiveBuffer
                if (isTimeout && _receiveBufferLength > 0)
                {
                    EnqueueNonPacketBytesEvent(time, ReceiveBuffer[0.._receiveBufferLength]);

                    _receiveBufferLength = 0;
                    _parseWindowStart = 0;
                }

                if (!EventQueue.IsEmpty)
                {
                    RaiseEvent();
                }
            }
        }

        public void Terminate()
        {
            _timer.Stop();

            lock (_lock)
            {
                TerminateInternal();

                _receiveBufferLength = 0;
                _parseWindowStart = 0;
            }
        }

        protected abstract PacketMode CreateCloneInternal();

        protected abstract void ValidateInternal();

        protected abstract void BytesReceivedInternal(DateTime now);

        protected abstract void TerminateInternal();

        protected abstract int ComputePacketLength(ReadOnlySpan<byte> window);

        private void TestSimulatedPacket()
        {
            List<byte> bytes = [];
            int indexNow = 0;
            Dictionary<int, int> fieldIndexToLengthTable = [];
            Dictionary<int, int> lengthFieldIndexToByteIndexTable = [];

            for (int i = 0; i < Fields.Count; ++i)
            {
                PacketField packetField = Fields[i];

                int fieldLength;
                if (packetField.LengthMode == LengthMode.FixedLength)
                {
                    fieldLength = packetField.FixedLength;
                    for (int whichByte = 0; whichByte < fieldLength; ++whichByte)
                    {
                        bytes.Add(0);
                    }

                    if (packetField is LengthField)
                    {
                        lengthFieldIndexToByteIndexTable[i] = indexNow;
                    }
                }
                else if (packetField.LengthMode == LengthMode.FixedData)
                {
                    fieldLength = packetField.Data.Length;
                    bytes.AddRange(packetField.Data);
                }
                else if (packetField.LengthMode == LengthMode.VariableLength)
                {
                    fieldLength = 1;
                    for (int whichByte = 0; whichByte < fieldLength; ++whichByte)
                    {
                        bytes.Add(0);
                    }
                }
                else
                {
                    throw new Exception("Invalid Enum");
                }

                fieldIndexToLengthTable[i] = fieldLength;

                indexNow += fieldLength;
            }

            for (int i = 0; i < Fields.Count; ++i)
            {
                PacketField packetField = Fields[i];

                int lengthValue = 0;
                if (packetField is LengthField lengthField)
                {
                    for (int lengthIndex = lengthField.StartFieldIndex; lengthIndex <= lengthField.EndFieldIndex; ++lengthIndex)
                    {
                        lengthValue += fieldIndexToLengthTable[lengthIndex];
                    }

                    lengthValue -= lengthField.ValueOffset;

                    int byteIndex = lengthFieldIndexToByteIndexTable[i];

                    ((long)lengthValue).FillBytes(bytes, byteIndex, lengthField.FixedLength);
                }
            }

            ParseResultData parseResultData = new ParseResultData();
            _parsedFields = [];
            for (int i = 0; i < Fields.Count; ++i)
            {
                _parsedFields.Add(new ParsePacketField(Fields[i]));
            }

            ParseResult result = ParsePacket(new ReadOnlySpan<byte>([.. bytes]), _parsedFields, parseResultData);

            if (result != ParseResult.FullPacket)
            {
                throw new Exception("Invalid Protocol.");
            }
        }

        private ParseResult ParsePacket(ReadOnlySpan<byte> packetBytes, List<ParsePacketField> fields, ParseResultData parseResult)
        {
            ParseResult result = ParseResult.ErrorPacket;
            List<LengthInfo> pendingLengthFields = [];
            int fieldIndex = 0;
            int bytesIndex = 0;
            bool advanced = true;
            LengthResolveResult lengthResolveResult;

            int modePacketLength = ComputePacketLength(packetBytes);

            if (modePacketLength > 0 && packetBytes.Length < modePacketLength)
            {
                result = ParseResult.WaitForFullPacket;
            }
            else
            {
                fields[0].TrySetResolvedStartIndex(0);

                if (modePacketLength > 0)
                {
                    pendingLengthFields.Add(new LengthInfo(0, fields.Count - 1, modePacketLength));
                }

                while (advanced)
                {
                    advanced = false;
                    while (true)
                    {
                        if (bytesIndex >= packetBytes.Length)
                        {
                            result = ParseResult.WaitForFullPacket;
                            goto Finish;
                        }

                        ParsePacketField field = fields[fieldIndex];
                        int fieldLength;

                        if (field.LengthMode == LengthMode.FixedLength)
                        {
                            fieldLength = field.FixedLength;
                            if (field.Field is LengthField lengthField)
                            {
                                int lengthValue = ((int)packetBytes.ToUint(bytesIndex, lengthField.FixedLength)) + lengthField.ValueOffset;
                                pendingLengthFields.Add(new LengthInfo(lengthField, lengthValue));
                            }
                        }
                        else if (field.LengthMode == LengthMode.FixedData)
                        {
                            fieldLength = field.Data.Length;
                        }
                        else if (field.LengthMode == LengthMode.VariableLength)
                        {
                            if (field.ResolvedLength > 0)
                            {
                                fieldLength = field.ResolvedLength;
                            }
                            else
                            {
                                if (!field.CoveredByLengthField || field.NeedSearchFixedData == NeedSearchFixedDataState.Need)
                                {
                                    if (fieldIndex < fields.Count - 1
                                        && fields[fieldIndex + 1].LengthMode == LengthMode.FixedData)
                                    {
                                        ParsePacketField nextField = fields[fieldIndex + 1];
                                        int targetIndex = packetBytes[bytesIndex..].IndexOf(nextField.Data);
                                        if (targetIndex >= 0)
                                        {
                                            fieldLength = targetIndex;
                                        }
                                        else
                                        {
                                            result = ParseResult.WaitForFullPacket;
                                            goto Finish;
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("Invalid Protocol");
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Invalid Enum");
                        }

                        if (!field.TrySetResolvedLength(fieldLength))
                        {
                            result = ParseResult.ErrorPacket;
                            parseResult.ParseErrorType = ParseErrorType.LengthFieldsConflict;
                            goto Finish;
                        }

                        if (fieldIndex < fields.Count - 1)
                        {
                            if (!fields[fieldIndex + 1].TrySetResolvedStartIndex(field.ResolvedStartIndex + field.ResolvedLength))
                            {
                                result = ParseResult.ErrorPacket;
                                parseResult.ParseErrorType = ParseErrorType.LengthFieldsConflict;
                                goto Finish;
                            }
                        }

                        ++fieldIndex;
                        bytesIndex += fieldLength;

                        if (field.ResolvedStartIndex + field.ResolvedLength > packetBytes.Length)
                        {
                            result = ParseResult.WaitForFullPacket;
                            goto Finish;
                        }

                        field.ActualData = packetBytes.Slice(field.ResolvedStartIndex, field.ResolvedLength).ToArray();

                        advanced = true;

                        if (fieldIndex >= fields.Count)
                        {
                            lengthResolveResult = ProcessLengthFields(pendingLengthFields, fields);
                            if ((lengthResolveResult == LengthResolveResult.Advanced || lengthResolveResult == LengthResolveResult.Empty)
                                && pendingLengthFields.Count <= 0)
                            {
                                result = ParseResult.FullPacket;
                                parseResult.PacketLength = bytesIndex;
                                goto Finish;
                            }
                            else
                            {
                                result = ParseResult.ErrorPacket;
                                parseResult.ParseErrorType = ParseErrorType.EopAndFieldsConflict;
                                goto Finish;
                            }
                        }
                    }

                    lengthResolveResult = ProcessLengthFields(pendingLengthFields, fields);

                    if (lengthResolveResult == LengthResolveResult.Conflict)
                    {
                        result = ParseResult.ErrorPacket;
                        parseResult.ParseErrorType = ParseErrorType.LengthFieldsConflict;
                        goto Finish;
                    }
                    else if (lengthResolveResult == LengthResolveResult.Advanced)
                    {
                        advanced = true;
                    }
                }

            Finish:
                ;
            }

            return result;
        }

        private static LengthResolveResult ProcessLengthFields(List<LengthInfo> pendingLengthFields, List<ParsePacketField> fields)
        {
            LengthResolveResult result = LengthResolveResult.Unknown;

            if (pendingLengthFields.Count <= 0)
            {
                result = LengthResolveResult.Empty;
            }
            else
            {
                for (int whichLengthField = pendingLengthFields.Count - 1; whichLengthField >= 0; --whichLengthField)
                {
                    LengthInfo lengthInfo = pendingLengthFields[whichLengthField];
                    for (int i = lengthInfo.ParseStartIndex; i < lengthInfo.ParseEndIndex; ++i)
                    {
                        ParsePacketField packetField = fields[i];
                        int fieldLength;
                        if (packetField.LengthMode == LengthMode.FixedLength)
                        {
                            fieldLength = packetField.FixedLength;
                        }
                        else if (packetField.LengthMode == LengthMode.FixedData)
                        {
                            fieldLength = packetField.Data.Length;
                        }
                        else if (packetField.LengthMode == LengthMode.VariableLength && packetField.Resolved)
                        {
                            fieldLength = packetField.ResolvedLength;
                            fields[i].NeedSearchFixedData = NeedSearchFixedDataState.NoNeed;
                        }
                        else
                        {
                            break;
                        }

                        if (lengthInfo.ParseStartIndex < fields.Count - 1)
                        {
                            if (!fields[lengthInfo.ParseStartIndex + 1].TrySetResolvedStartIndex(fields[lengthInfo.ParseStartIndex].ResolvedEndIndex + 1))
                            {
                                result = LengthResolveResult.Conflict;
                                goto Finish;
                            }
                        }

                        ++lengthInfo.ParseStartIndex;
                        lengthInfo.ResolvedLengthFieldLength -= fieldLength;
                    }

                    for (int i = lengthInfo.ParseEndIndex; i > lengthInfo.ParseStartIndex; --i)
                    {
                        ParsePacketField packetField = fields[i];
                        int fieldLength;
                        if (packetField.LengthMode == LengthMode.FixedLength)
                        {
                            fieldLength = packetField.FixedLength;
                        }
                        else if (packetField.LengthMode == LengthMode.FixedData)
                        {
                            fieldLength = packetField.Data.Length;
                        }
                        else if (packetField.LengthMode == LengthMode.VariableLength && packetField.Resolved)
                        {
                            fieldLength = packetField.ResolvedLength;
                        }
                        else
                        {
                            break;
                        }

                        if (lengthInfo.ParseEndIndex > 0)
                        {
                            if (!fields[lengthInfo.ParseEndIndex - 1].TrySetResolvedEndIndex(fields[lengthInfo.ParseEndIndex].ResolvedStartIndex - 1))
                            {
                                result = LengthResolveResult.Conflict;
                                goto Finish;
                            }
                        }

                        --lengthInfo.ParseEndIndex;
                        lengthInfo.ResolvedLengthFieldLength -= fieldLength;
                    }

                    if (lengthInfo.ParseStartIndex == lengthInfo.ParseEndIndex)
                    {
                        bool setSuccess = fields[lengthInfo.ParseStartIndex].TrySetResolvedLength(lengthInfo.ResolvedLengthFieldLength);

                        if (!setSuccess)
                        {
                            result = LengthResolveResult.Conflict;
                            goto Finish;
                        }
                        else
                        {
                            pendingLengthFields.RemoveAt(whichLengthField);
                            result = LengthResolveResult.Advanced;
                        }
                    }
                    else
                    {
                        if (fields[lengthInfo.ParseStartIndex].NeedSearchFixedData == NeedSearchFixedDataState.Unknown)
                        {
                            fields[lengthInfo.ParseStartIndex].NeedSearchFixedData = NeedSearchFixedDataState.Need;
                            result = LengthResolveResult.Advanced;
                        }
                    }
                }
            }

        Finish:
            ;

            return result;
        }

        private void EnqueueNonPacketBytesEvent(DateTime time, byte[] bytes)
        {
            NonPacketBytesReceived nonPacketBytesEvent = new NonPacketBytesReceived(time, bytes);
            EventQueue.Enqueue(nonPacketBytesEvent);
        }

        private void EnqueuePacketEvent(DateTime time, List<ParsePacketField> fields, byte[] bytes)
        {
            PacketReceived packetEvent = new PacketReceived(time, fields, bytes);
            EventQueue.Enqueue(packetEvent);
        }

        private void RaiseEvent()
        {
            Task.Run(() =>
            {
                DataReceived?.Invoke(this, EventArgs.Empty);
            });
        }

        private void _timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            ParsePackets(DateTime.Now, true);
        }

        private void _fields_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            for (int i = 0; i < Fields.Count; ++i)
            {
                Fields[i].Index = i;
            }
        }
    }
}
