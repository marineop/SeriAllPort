using CommonWpf.Communication.Protocol.PacketFields;

namespace CommonWpf.Communication.Protocol.ParseData
{
    public class ParsePacketField : ViewModel
    {
        private PacketField _field;
        public PacketField Field
        {
            get => _field;
            private set
            {
                _field = value;
            }
        }

        public string Name => _field.Name;

        public LengthMode LengthMode => _field.LengthMode;
        public byte[] Data => _field.Data;
        public int FixedLength => _field.FixedLength;

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

        private byte[] _actualData = [];
        public byte[] ActualData
        {
            get => _actualData;
            set
            {
                if (_actualData != value)
                {
                    _actualData = value;
                    OnPropertyChanged();
                }
            }
        }

        internal bool CoveredByLengthField => _field.CoveredByLengthField;

        internal int ResolvedStartIndex { get; private set; } = -1;

        internal int ResolvedEndIndex { get; private set; } = -1;

        internal int ResolvedLength { get; private set; } = 0;

        internal bool Resolved => ResolvedStartIndex >= 0 && ResolvedLength > 0;

        internal NeedSearchFixedDataState NeedSearchFixedData { get; set; } = NeedSearchFixedDataState.Unknown;

        public ParsePacketField(PacketField packetField)
        {
            _field = packetField;
        }

        public override string ToString()
        {
            return $"{Field.TypeName}, {Name}: {LengthMode}, Resolve=({ResolvedStartIndex}, {ResolvedEndIndex}, {ResolvedLength})";
        }

        internal void ResetResolveData()
        {
            ResolvedStartIndex = -1;
            ResolvedEndIndex = -1;
            ResolvedLength = -1;
            NeedSearchFixedData = NeedSearchFixedDataState.Unknown;
        }

        internal bool TrySetResolvedStartIndex(int index)
        {
            bool answer = true;
            if (ResolvedStartIndex >= 0 && ResolvedStartIndex != index)
            {
                answer = false;
            }
            else
            {

                if (ResolvedEndIndex < 0)
                {
                    if (ResolvedLength > 0)
                    {
                        ResolvedEndIndex = index + ResolvedLength - 1;
                    }
                    else // ResolvedLength <= 0
                    {
                    }
                }
                else // ResolvedEndIndex >= 0
                {
                    if (ResolvedLength > 0)
                    {
                        int tempResolvedEndIndex = index + ResolvedLength - 1;
                        if (ResolvedEndIndex != tempResolvedEndIndex)
                        {
                            answer = false;
                        }
                    }
                    else // ResolvedLength <= 0
                    {
                        ResolvedLength = ResolvedEndIndex - index + 1;
                    }
                }
            }

            if (answer)
            {
                ResolvedStartIndex = index;
            }

            return answer;
        }

        internal bool TrySetResolvedEndIndex(int index)
        {
            bool answer = true;
            if (ResolvedEndIndex >= 0 && ResolvedEndIndex != index)
            {
                answer = false;
            }
            else
            {

                if (ResolvedStartIndex < 0)
                {
                    if (ResolvedLength > 0)
                    {
                        ResolvedStartIndex = index - ResolvedLength + 1;
                    }
                    else // ResolvedLength <= 0
                    {
                    }
                }
                else // ResolvedStartIndex >= 0
                {
                    if (ResolvedLength > 0)
                    {
                        int tempResolvedStartIndex = index - ResolvedLength + 1;
                        if (ResolvedStartIndex != tempResolvedStartIndex)
                        {
                            answer = false;
                        }
                    }
                    else // ResolvedLength <= 0
                    {
                        ResolvedLength = index - ResolvedStartIndex + 1;
                    }
                }
            }

            if (answer)
            {
                ResolvedEndIndex = index;
            }

            return answer;
        }

        internal bool TrySetResolvedLength(int length)
        {
            bool answer = true;
            if (ResolvedLength > 0 && ResolvedLength != length)
            {
                answer = false;
            }
            else
            {

                if (ResolvedStartIndex < 0)
                {
                    if (ResolvedEndIndex >= 0)
                    {
                        ResolvedStartIndex = ResolvedEndIndex - length + 1;
                    }
                    else // ResolvedEndIndex < 0
                    {
                    }
                }
                else // ResolvedStartIndex >= 0
                {
                    if (ResolvedEndIndex >= 0)
                    {
                        int tempResolvedStartIndex = ResolvedEndIndex - length + 1;
                        if (ResolvedStartIndex != tempResolvedStartIndex)
                        {
                            answer = false;
                        }
                    }
                    else // ResolvedEndIndex < 0
                    {
                        ResolvedEndIndex = ResolvedStartIndex + length - 1;
                    }
                }
            }

            if (answer)
            {
                ResolvedLength = length;
            }

            return answer;
        }
    }
}
