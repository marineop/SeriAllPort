using CommonWpf.Communication.Protocol.PacketFields;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CommonWpf.Communication.ErrorDetection
{
    [JsonDerivedType(typeof(CRC), typeDiscriminator: "CRC")]
    [JsonDerivedType(typeof(Checksum), typeDiscriminator: "Checksum")]
    [JsonDerivedType(typeof(Xor), typeDiscriminator: "Xor")]
    public interface IErrorDetection : INotifyPropertyChanged
    {
        string Name { get; }
        bool CanEdit { get; }

        int ByteCount { get; }

        int ComputeErrorDetectionCode(byte[] input, int startIndex, int length, byte[] errorDetectionCode, Endianness endianness);
    }
}
