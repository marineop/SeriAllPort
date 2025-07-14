namespace CommonWpf.Communication.Prococol.EventTypes
{
    public class NonPacketBytesReceived : PacketEventType
    {
        public NonPacketBytesReceived(byte[] bytes)
            : base(bytes)
        {
        }
    }
}
