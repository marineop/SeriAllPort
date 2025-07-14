using CommonWpf;
using System.Collections.ObjectModel;

namespace SeriAllPort.ViewModels.SendRawData
{
    public class SendRawDataViewModel : ViewModel
    {
        private ITextToBytes _SendFormat;
        public ITextToBytes SendFormat
        {
            get => _SendFormat;
            set
            {
                if (_SendFormat != value)
                {
                    _SendFormat = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<ITextToBytes> _SendFormats;
        public ObservableCollection<ITextToBytes> SendFormats
        {
            get => _SendFormats;
            set
            {
                if (_SendFormats != value)
                {
                    _SendFormats = value;
                    OnPropertyChanged();
                }
            }
        }

        public SendRawDataViewModel()
        {
            _SendFormats =
            [
                new SendFormatBytes(),
                new SendFormatString()
            ];

            _SendFormat = SendFormats[0];
        }
    }
}
