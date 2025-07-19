using CommonWpf.ViewModels.TextBytes;

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

        public Preamble(string name, TextBytesViewModel textBytes)
             : base(name, LengthMode.FixedData, textBytes, textBytes.Bytes.Length)
        {
        }

        public override PacketField CreateClone()
        {
            Preamble newPacketField = new Preamble(Name, TextBytes);

            AssignWithSelfValue(newPacketField);

            return newPacketField;
        }
    }
}
