namespace CommonWpf.Communication.Protocol.EventTypes
{
    public class NonPacketBytesReceived : PacketEventType
    {
        public NonPacketBytesReceived(byte[] bytes)
            : base(bytes)
        {
        }
    }
}
