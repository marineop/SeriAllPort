using CommonWpf;
using System.Text;

namespace SeriAllPort.ViewModels.SendRawData
{
    public class SendFormatString : ViewModel, ITextToBytes
    {
        public string Name => "Text";

        private string? _Text;
        public string? Text
        {
            get => _Text;
            set
            {
                if (_Text != value)
                {
                    _Text = value;
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
                return new byte[0];
            }
        }
    }
}
