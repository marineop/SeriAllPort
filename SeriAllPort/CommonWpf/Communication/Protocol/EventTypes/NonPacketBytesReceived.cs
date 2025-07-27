namespace CommonWpf.Communication.Protocol.EventTypes
{
    public class NonPacketBytesReceived : PacketEventType
    {
        internal NonPacketBytesReceived(DateTime time, byte[] bytes)
            : base(time, bytes)
        {
        }
    }
}
