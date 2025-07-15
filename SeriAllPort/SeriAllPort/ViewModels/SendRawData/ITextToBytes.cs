using System.Text.Json.Serialization;

namespace SeriAllPort.ViewModels.SendRawData
{
    [JsonDerivedType(typeof(SendFormatBytes), typeDiscriminator: "Bytes")]
    [JsonDerivedType(typeof(SendFormatText), typeDiscriminator: "Text")]
    public interface ITextToBytes
    {
        byte[] GetBytes();
    }
}
