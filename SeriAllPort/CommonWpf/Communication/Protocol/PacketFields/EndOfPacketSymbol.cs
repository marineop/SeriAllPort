﻿using CommonWpf.ViewModels.TextBytes;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public override bool CanEditLengthMode { get; } = false;

        public EndOfPacketSymbol(string name, byte[] bytes)
            : base(name,
                  LengthMode.FixedData,
                  new TextBytes(TextRepresentation.Bytes, bytes),
                  bytes.Length)
        {
        }

        [JsonConstructor]
        public EndOfPacketSymbol(string name, TextBytes textBytes)
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
