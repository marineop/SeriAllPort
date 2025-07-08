using CommonWpf;
using CommonWpf.Extensions;

namespace SeriAllPort.ViewModels.SendRawData
{
    public class SendFormatBytes : ViewModel, ITextToBytes
    {
        public string Name => "Bytes";

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
                return Text.HexStringToBytes();
            }
            else
            {
                return new byte[0];
            }
        }
    }
}
