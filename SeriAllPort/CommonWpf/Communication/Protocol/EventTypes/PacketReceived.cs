using CommonWpf.Communication.Protocol.PacketFields;

namespace CommonWpf.Communication.Protocol.EventTypes
{
    public class PacketReceived : PacketEventType
    {
        public List<PacketField> PacketFields { get; private set; }

        public PacketReceived(
            DateTime time,
            List<PacketField> packetFields,
            byte[] bytes)
            : base(time, bytes)
        {
            PacketFields = packetFields;
        }
    }
}
