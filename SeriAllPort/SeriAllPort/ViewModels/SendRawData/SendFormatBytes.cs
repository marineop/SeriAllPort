using CommonWpf;
using CommonWpf.Extensions;
using System.Text.Json.Serialization;

namespace SeriAllPort.ViewModels.SendRawData
{
    public class SendFormatBytes : ViewModel, ITextToBytes
    {
        [JsonIgnore]
        public static string Name => "Bytes";

        private string? _text;
        public string? Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged();
                }
            }
        }

        public byte[] GetBytes()
        {
            if (Text != null)
            {
                return Text.HexStringToBytes();
            }
            else
            {
                return [];
            }
        }
    }
}
