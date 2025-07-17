namespace CommonWpf.Communication.Protocol.EventTypes
{
    public class NonPacketBytesReceived : PacketEventType
    {
        public NonPacketBytesReceived(DateTime time, byte[] bytes)
            : base(time, bytes)
        {
        }
    }
}
