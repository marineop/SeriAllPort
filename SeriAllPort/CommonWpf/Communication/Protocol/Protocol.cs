using CommonWpf.Communication.Protocol.PacketModes;
using CommonWpf.FileHelper;

namespace CommonWpf.Communication.Protocol
{
    public class Protocol : ViewModel, IAppDataFolderFile
    {
        public static string AppDataSubFolder => "Protocols";

        public static string FileExtensionName => "ptc";

        public Guid Id { get; set; }

        public int Order { get; set; }

        private string _name;
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

        private PacketMode _packetMode;
        public PacketMode PacketMode
        {
            get => _packetMode;
            set
            {
                if (_packetMode != value)
                {
                    _packetMode = value ?? throw new Exception("Packet Mode cannot be assigned to null");
                    OnPropertyChanged();
                }
            }
        }

        public Protocol(
            string name,
            Guid id,
            PacketMode packetMode)
        {
            _name = name;
            Id = id;
            _packetMode = packetMode;
        }

        public Protocol CreateClone()
        {
            Protocol newProtocol = (Protocol)MemberwiseClone();

            newProtocol.PacketMode = PacketMode.CreateClone();

            return newProtocol;
        }
    }
}
