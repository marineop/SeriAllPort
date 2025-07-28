using CommonWpf.ViewModels.TextBytes;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public override bool CanEditLengthMode { get; } = false;

        public Preamble(string name, byte[] bytes)
             : base(name,
                   LengthMode.FixedData,
                   new TextBytes(TextRepresentation.Bytes, bytes),
                   bytes.Length)
        {
        }

        [JsonConstructor]
        public Preamble(string name, TextBytes textBytes)
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
