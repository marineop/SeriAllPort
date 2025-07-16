namespace CommonWpf.Communication.Protocol.PacketFields
{
    public class EndOfPacketSymbol : PacketField
    {
        public override string TypeName => "End of Packet";

        public override LengthMode LengthMode
        {
            get => base.LengthMode;
            set => base.LengthMode = LengthMode.FixedData;
        }

        public EndOfPacketSymbol(string name, byte[] data)
            : base(name, LengthMode.FixedData, data, data.Length)
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
