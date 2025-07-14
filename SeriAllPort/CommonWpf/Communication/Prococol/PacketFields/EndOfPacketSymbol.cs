namespace CommonWpf.Communication.Prococol.PacketFields
{
    public class EndOfPacketSymbol : PacketField
    {
        public override string TypeName => "End of Packet";

        public override bool IsFixedLength
        {
            get => base.IsFixedLength;
            set => base.IsFixedLength = true;
        }

        public EndOfPacketSymbol()
        {
        }

        public override PacketField CreateClone()
        {
            EndOfPacketSymbol newPacketField = new EndOfPacketSymbol();

            AssignWithSelfValue(newPacketField);

            return newPacketField;
        }
    }
}
