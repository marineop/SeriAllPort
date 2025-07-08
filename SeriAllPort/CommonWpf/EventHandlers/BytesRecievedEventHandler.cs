namespace CommonWpf.EventHandlers
{
    public delegate void BytesReceivedEventHandler(object? sender, BytesReceivedEventArgs e);

    public class BytesReceivedEventArgs : EventArgs
    {
        public byte[] Bytes { get; set; }

        public int Offset { get; set; }

        public int Length { get; set; }

        public BytesReceivedEventArgs(byte[] bytes, int offset, int length)
        {
            Bytes = bytes;
            Offset = offset;
            Length = length;
        }
    }
}
