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

        public EndOfPacketSymbol(string name, byte[] data)
            : base(name, true, data, data.Length)
        {
        }

        public override PacketField CreateClone()
        {
            EndOfPacketSymbol newPacketField = new EndOfPacketSymbol(Name, Data);

            AssignWithSelfValue(newPacketField);

            return newPacketField;
        }
    }
}
