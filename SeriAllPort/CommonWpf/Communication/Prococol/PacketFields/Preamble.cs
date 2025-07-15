using System.Xml.Linq;

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

        public Preamble(string name, byte[] data)
             : base(name, true, data, data.Length)
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
