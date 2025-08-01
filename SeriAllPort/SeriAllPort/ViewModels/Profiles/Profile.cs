﻿using CommonWpf;
using CommonWpf.Communication.PhysicalInterfaces;
using CommonWpf.FileHelper;
using CommonWpf.ViewModels.ListEditor;
using SeriAllPort.ViewModels.Commands;
using SeriAllPort.ViewModels.SendRawData;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace SeriAllPort.ViewModels.Profiles
{
    public class Profile : ViewModel, IAppDataFolderFile, IListEditorItem
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

        private bool _canNotEditName = false;
        public bool CanNotEditName
        {
            get => _canNotEditName;
            set
            {
                if (_canNotEditName != value)
                {
                    _canNotEditName = value;
                    OnPropertyChanged();
                }
            }
        }

        public Guid ProtocolId { get; set; } = Guid.Empty;

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _logDisplayBytes = true;
        public bool LogDisplayBytes
        {
            get => _logDisplayBytes;
            set
            {
                if (_logDisplayBytes != value)
                {
                    _logDisplayBytes = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _logDisplayText = false;
        public bool LogDisplayText
        {
            get => _logDisplayText;
            set
            {
                if (_logDisplayText != value)
                {
                    _logDisplayText = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _logDisplayParsed = false;
        public bool LogDisplayParsed
        {
            get => _logDisplayParsed;
            set
            {
                if (_logDisplayParsed != value)
                {
                    _logDisplayParsed = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _sendFormatIndex = 0;
        public int SendFormatIndex
        {
            get => _sendFormatIndex;
            set
            {
                if (_sendFormatIndex != value)
                {
                    _sendFormatIndex = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SendFormat));
                }
            }
        }

        private ObservableCollection<ITextToBytes> _sendFormats = [];
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

        private ObservableCollection<Command> _commands = [];
        public ObservableCollection<Command> Commands
        {
            get => _commands;
            set
            {
                if (_commands != value)
                {
                    _commands = value;
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
                new SendFormatText()
            ];
        }

        public Profile CreateClone()
        {
            Profile newProfile = (Profile)MemberwiseClone();

            newProfile.SendFormats = new ObservableCollection<ITextToBytes>(SendFormats);
            newProfile.ComPortSettings = ComPortSettings.Clone();

            return newProfile;
        }
    }
}
