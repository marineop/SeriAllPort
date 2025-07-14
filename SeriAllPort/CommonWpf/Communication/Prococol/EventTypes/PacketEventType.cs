namespace CommonWpf.Communication.Prococol.EventTypes
{
    public abstract class PacketEventType
    {
        public byte[] Bytes { get; private set; }

        protected PacketEventType(byte[] bytes)
        {
            Bytes = bytes;
        }
    }
}
