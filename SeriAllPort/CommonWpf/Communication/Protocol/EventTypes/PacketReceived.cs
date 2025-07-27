using CommonWpf.Communication.Protocol.ParseData;

namespace CommonWpf.Communication.Protocol.EventTypes
{
    public class PacketReceived : PacketEventType
    {
        public List<ParsePacketField> PacketFields { get; private set; }

        internal PacketReceived(
            DateTime time,
            List<ParsePacketField> packetFields,
            byte[] bytes)
            : base(time, bytes)
        {
            PacketFields = packetFields;
        }
    }
}
