using CommonWpf.Communication.Protocol.PacketFields;

namespace CommonWpf.Communication.Protocol.EventTypes
{
    public class PacketReceived : PacketEventType
    {
        public List<PacketField> PacketFields { get; private set; }

        public PacketReceived(List<PacketField> packetFields, byte[] bytes)
            : base(bytes)
        {
            PacketFields = packetFields;
        }
    }
}
