using CommonWpf;
using System.Text;
using System.Text.Json.Serialization;

namespace SeriAllPort.ViewModels.SendRawData
{
    public class SendFormatText : ViewModel, ITextToBytes
    {
        [JsonIgnore]
        public string Name => "Text";

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
                return Encoding.UTF8.GetBytes(Text);
            }
            else
            {
                return Array.Empty<byte>();
            }
        }
    }
}
