using CommonWpf.Communication.Prococol.PacketFields;

namespace CommonWpf.Communication.Prococol.EventTypes
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
