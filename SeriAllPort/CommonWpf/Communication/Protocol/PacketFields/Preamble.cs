namespace CommonWpf.Communication.Protocol.PacketFields
{
    public class Preamble : PacketField
    {
        public override string TypeName => "Preamble";

        public override LengthMode LengthMode
        {
            get => base.LengthMode;
            set => base.LengthMode = LengthMode.FixedData;
        }

        public Preamble(string name, byte[] data)
             : base(name, LengthMode.FixedData, data, data.Length)
        {
        }

        public override PacketField CreateClone()
        {
            Preamble newPacketField = new Preamble(Name, Data);

            AssignWithSelfValue(newPacketField);

            return newPacketField;
        }
    }
}
