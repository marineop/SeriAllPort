using CommonWpf.ViewModels.TextBytes;

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

        public EndOfPacketSymbol(string name, TextBytesViewModel textBytes)
            : base(name, LengthMode.FixedData, textBytes, textBytes.Bytes.Length)
        {
        }

        public override PacketField CreateClone()
        {
            EndOfPacketSymbol newPacketField = new EndOfPacketSymbol(Name, TextBytes);

            AssignWithSelfValue(newPacketField);

            return newPacketField;
        }
    }
}
