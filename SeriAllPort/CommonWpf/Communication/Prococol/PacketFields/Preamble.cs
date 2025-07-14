namespace CommonWpf.Communication.Prococol.PacketFields
{
    public class Preamble : PacketField
    {
        public override string TypeName => "Preamble";

        public override bool IsFixedLength
        {
            get => base.IsFixedLength;
            set => base.IsFixedLength = true;
        }

        public Preamble()
        {
        }

        public override PacketField CreateClone()
        {
            Preamble newPacketField = new Preamble();

            AssignWithSelfValue(newPacketField);

            return newPacketField;
        }
    }
}
