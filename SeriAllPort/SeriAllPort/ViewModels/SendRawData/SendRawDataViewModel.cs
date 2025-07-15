using CommonWpf;
using System.Collections.ObjectModel;

namespace SeriAllPort.ViewModels.SendRawData
{
    public class SendRawDataViewModel : ViewModel
    {
        private ITextToBytes _sendFormat;
        public ITextToBytes SendFormat
        {
            get => _sendFormat;
            set
            {
                if (_sendFormat != value)
                {
                    _sendFormat = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<ITextToBytes> _sendFormats;
        public ObservableCollection<ITextToBytes> SendFormats
        {
            get => _sendFormats;
            set
            {
                if (_sendFormats != value)
                {
                    _sendFormats = value;
                    OnPropertyChanged();
                }
            }
        }

        public SendRawDataViewModel()
        {
            _sendFormats =
            [
                new SendFormatBytes(),
                new SendFormatText()
            ];

            _sendFormat = SendFormats[0];
        }
    }
}
