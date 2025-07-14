using System.Text.Json.Serialization;

namespace SeriAllPort.ViewModels.SendRawData
{
    [JsonDerivedType(typeof(SendFormatBytes), typeDiscriminator: "Bytes")]
    [JsonDerivedType(typeof(SendFormatString), typeDiscriminator: "Text")]
    public interface ITextToBytes
    {
        byte[] GetBytes();
    }
}
