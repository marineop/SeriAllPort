using CommonWpf.Communication.Protocol.PacketFields;

namespace CommonWpf.Communication.Protocol.ParseData
{
    internal class LengthInfo
    {
        private readonly int _originalStartIndex;
        private readonly int _originalEndIndex;

        internal int ParseStartIndex { get; set; }
        internal int ParseEndIndex { get; set; }
        internal int ResolvedLengthFieldLength { get; set; } = 0;

        internal LengthInfo(LengthField lengthField, int length)
        {
            _originalStartIndex = lengthField.StartFieldIndex;
            _originalEndIndex = lengthField.EndFieldIndex;
            ResolvedLengthFieldLength = length;

            ParseStartIndex = _originalStartIndex;
            ParseEndIndex = _originalEndIndex;
        }

        internal LengthInfo(int startIndex, int endIndex, int length)
        {
            _originalStartIndex = startIndex;
            _originalEndIndex = endIndex;
            ResolvedLengthFieldLength = length;

            ParseStartIndex = _originalStartIndex;
            ParseEndIndex = _originalEndIndex;
        }
    }
}
