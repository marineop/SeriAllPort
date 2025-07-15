using CommonWpf;
using CommonWpf.Communication.PhysicalInterfaces;
using CommonWpf.FileHelper;
using SeriAllPort.ViewModels.SendRawData;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace SeriAllPort.ViewModels.Profiles
{
    public class Profile : ViewModel, IAppDataFolderFile
    {
        public static string AppDataSubFolder => "Profiles";

        public static string FileExtensionName => "pro";

        public Guid Id { get; set; }

        public int Order { get; set; }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _canNotDelete = false;
        public bool CanNotDelete
        {
            get => _canNotDelete;
            set
            {
                if (_canNotDelete != value)
                {
                    _canNotDelete = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _CanNotEditName = false;
        public bool CanNotEditName
        {
            get => _CanNotEditName;
            set
            {
                if (_CanNotEditName != value)
                {
                    _CanNotEditName = value;
                    OnPropertyChanged();
                }
            }
        }

        public Guid ProtocolId { get; set; } = Guid.Empty;

        private string _Description = string.Empty;
        public string Description
        {
            get => _Description;
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _displayBytes = true;
        public bool DisplayBytes
        {
            get => _displayBytes;
            set
            {
                if (_displayBytes != value)
                {
                    _displayBytes = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _displayText = true;
        public bool DisplayText
        {
            get => _displayText;
            set
            {
                if (_displayText != value)
                {
                    _displayText = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _SendFormatIndex = 0;
        public int SendFormatIndex
        {
            get => _SendFormatIndex;
            set
            {
                if (_SendFormatIndex != value)
                {
                    _SendFormatIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<ITextToBytes> _sendFormats = new ObservableCollection<ITextToBytes>();
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

        [JsonIgnore]
        public ITextToBytes SendFormat => SendFormats[SendFormatIndex];

        private ComPortSettings _comPortSettings = new ComPortSettings();
        public ComPortSettings ComPortSettings
        {
            get => _comPortSettings;
            set
            {
                if (_comPortSettings != value)
                {
                    _comPortSettings = value;
                    OnPropertyChanged();
                }
            }
        }

        public Profile(string name, Guid id)
        {
            _name = name;
            Id = id;

            SendFormats =
            [
                new SendFormatBytes(),
                new SendFormatString()
            ];
        }

        public Profile CreateClone()
        {
            Profile newProfile = (Profile)MemberwiseClone();

            newProfile.SendFormats = new ObservableCollection<ITextToBytes>(SendFormats);
            newProfile.ComPortSettings = ComPortSettings.Clone();

            // TODO: more members

            return newProfile;
        }
    }
}
