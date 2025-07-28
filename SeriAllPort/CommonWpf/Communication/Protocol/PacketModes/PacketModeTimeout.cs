using CommonWpf.Communication.Protocol.PacketFields;
using CommonWpf.ViewModels.TextBytes;
using System.Text.Json.Serialization;

namespace CommonWpf.Communication.Protocol.PacketModes
{
    public class PacketModeTimeout : PacketMode
    {
        [JsonIgnore]
        public override string Name => "Timeout";

        public PacketModeTimeout()
            : base()
        {
        }

        public static PacketModeTimeout CreateDefault()
        {
            PacketModeTimeout packetModeTimeout = new PacketModeTimeout();

            PacketField packetField = new PacketField(
                "Data",
                LengthMode.VariableLength,
                new TextBytes(),
                0);

            packetModeTimeout.Fields.Add(packetField);

            return packetModeTimeout;
        }

        protected override void BytesReceivedInternal(DateTime time)
        {
        }

        protected override void ValidateInternal()
        {
            foreach (PacketField field in Fields)
            {
                if (field is EndOfPacketSymbol)
                {
                    throw new Exception("The EOP field cannot be used in Timeout mode.");
                }

                field.CoveredByLengthField = true;
            }
        }

        protected override void TerminateInternal()
        {
        }

        protected override PacketMode CreateCloneInternal()
        {
            PacketModeTimeout newMode = new PacketModeTimeout();

            return newMode;
        }

        protected override int ComputePacketLength(ReadOnlySpan<byte> window)
        {
            return window.Length;
        }
    }
}
