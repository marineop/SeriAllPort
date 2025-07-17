namespace CommonWpf.Communication.Protocol.EventTypes
{
    public abstract class PacketEventType
    {
        public DateTime Time { get; private set; }

        public byte[] Bytes { get; private set; }

        protected PacketEventType(DateTime time, byte[] bytes)
        {
            Time = time;
            Bytes = bytes;
        }
    }
}
