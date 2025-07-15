using CommonWpf;
using CommonWpf.Communication.Prococol;
using CommonWpf.Communication.Prococol.PacketFields;
using CommonWpf.Communication.Prococol.PacketModes;
using CommonWpf.FileHelper;
using System.Collections.ObjectModel;

namespace SeriAllPort.ViewModels.Protocols
{
    public class ProtocolEditorViewModel : ViewModel
    {
        private ObservableCollection<Protocol> _protocols = new ObservableCollection<Protocol>();
        public ObservableCollection<Protocol> Protocols
        {
            get => _protocols;
            set
            {
                if (_protocols != value)
                {
                    _protocols = value;
                    OnPropertyChanged();
                }
            }
        }

        private Protocol? _selectedProtocol;
        public Protocol? SelectedProtocol
        {
            get => _selectedProtocol;
            set
            {
                if (_selectedProtocol != value)
                {
                    _selectedProtocol = value;
                    OnPropertyChanged();

                    ProtocolUpCommand.RaiseCanExecuteChangedEvent();
                    ProtocolDownCommand.RaiseCanExecuteChangedEvent();
                    ProtocolDeleteCommand.RaiseCanExecuteChangedEvent();

                    FieldValidateCommand.RaiseCanExecuteChangedEvent();

                    if (_selectedProtocol != null)
                    {
                        PacketModes = new ObservableCollection<PacketMode>()
                        {
                            new PacketModeTimeout(),
                            new PacketModeEndOfPacketSymbol(),
                            _selectedProtocol.PacketMode,
                        };

                        for (int i = 0; i < PacketModes.Count; ++i)
                        {
                            if (PacketModes[i].GetType() == _selectedProtocol.PacketMode.GetType())
                            {
                                PacketModes.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }
            }
        }

        private int _selectedProtocolIndex;
        public int SelectedProtocolIndex
        {
            get => _selectedProtocolIndex;
            set
            {
                if (_selectedProtocolIndex != value)
                {
                    _selectedProtocolIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        private PacketField? _selectedPacketField;
        public PacketField? SelectedPacketField
        {
            get => _selectedPacketField;
            set
            {
                if (_selectedPacketField != value)
                {
                    _selectedPacketField = value;
                    OnPropertyChanged();

                    FieldUpCommand.RaiseCanExecuteChangedEvent();
                    FieldDownCommand.RaiseCanExecuteChangedEvent();
                    FieldDeleteCommand.RaiseCanExecuteChangedEvent();
                }
            }
        }

        private int _selectedPacketFieldIndex;
        public int SelectedPacketFieldIndex
        {
            get => _selectedPacketFieldIndex;
            set
            {
                if (_selectedPacketFieldIndex != value)
                {
                    _selectedPacketFieldIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _validateResult = "N/A";
        public string ValidateResult
        {
            get => _validateResult;
            set
            {
                if (_validateResult != value)
                {
                    _validateResult = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<PacketMode> _packetModes;
        public ObservableCollection<PacketMode> PacketModes
        {
            get => _packetModes;
            set
            {
                if (_packetModes != value)
                {
                    _packetModes = value;
                    OnPropertyChanged();
                }
            }
        }

        private IShowErrorDialog ShowErrorDialog { get; set; }

        public SimpleCommand ProtocolUpCommand { get; private set; }
        public SimpleCommand ProtocolDownCommand { get; private set; }
        public SimpleCommand ProtocolNewCommand { get; private set; }
        public SimpleCommand ProtocolDeleteCommand { get; private set; }

        public SimpleCommand FieldUpCommand { get; private set; }
        public SimpleCommand FieldDownCommand { get; private set; }
        public SimpleCommand FieldNewCommand { get; private set; }
        public SimpleCommand FieldDeleteCommand { get; private set; }
        public SimpleCommand FieldValidateCommand { get; private set; }

        public ProtocolEditorViewModel(
            IList<Protocol> protocols,
            Protocol currentProtocol,
            IShowErrorDialog showErrorDialog)
        {
            Protocol? selectedProtocol = null;
            foreach (Protocol protocol in protocols)
            {
                Protocol copy = protocol.CreateClone();
                Protocols.Add(copy);

                if (protocol.Id == currentProtocol.Id)
                {
                    selectedProtocol = copy;
                }
            }

            ShowErrorDialog = showErrorDialog;

            ProtocolNewCommand = new SimpleCommand((parameter) => ProtocolNew());

            ProtocolUpCommand = new SimpleCommand(
                (parameter) => ProtocolUp(),
                (parameter) => SelectedProtocol != null);

            ProtocolDownCommand = new SimpleCommand(
                (parameter) => ProtocolDown(),
                (parameter) => SelectedProtocol != null);

            ProtocolDeleteCommand = new SimpleCommand(
                (parameter) => ProtocolDelete(),
                (parameter) =>
                {
                    return SelectedProtocol != null && !SelectedProtocol.CanNotDelete;
                });

            FieldUpCommand = new SimpleCommand(
                (parameter) => FieldUp(),
                (parameter) => SelectedPacketField != null);

            FieldDownCommand = new SimpleCommand(
                (parameter) => FieldDown(),
                (parameter) => SelectedPacketField != null);

            FieldNewCommand = new SimpleCommand(
                (parameter) => FieldNew((Type?)parameter));

            FieldDeleteCommand = new SimpleCommand(
                (parameter) => FieldDelete(),
                (parameter) => SelectedPacketField != null);

            FieldValidateCommand = new SimpleCommand(
                (parameter) => FieldValidate(),
                (parameter) => SelectedProtocol != null);

            _packetModes =
            [
                new PacketModeTimeout(),
                new PacketModeEndOfPacketSymbol(),
            ];

            SelectedProtocol = selectedProtocol;
        }

        private void ProtocolUp()
        {
            if (SelectedProtocolIndex >= 0)
            {
                IList<Protocol>? protocols = Protocols;
                int index = SelectedProtocolIndex;

                if (index > 0)
                {
                    Protocol selectedProtocolPrevious = protocols[index - 1];
                    protocols.RemoveAt(index - 1);
                    protocols.Insert(index, selectedProtocolPrevious);
                }
            }
        }

        private void ProtocolDown()
        {
            if (SelectedProtocolIndex >= 0)
            {
                IList<Protocol>? protocols = Protocols;
                int index = SelectedProtocolIndex;

                if (index < protocols.Count - 1)
                {
                    Protocol selectedProtocolNext = protocols[index + 1];
                    protocols.RemoveAt(index + 1);
                    protocols.Insert(index, selectedProtocolNext);
                }
            }
        }

        private void ProtocolNew()
        {
            PacketModeTimeout packetMode = new PacketModeTimeout();

            List<string> protocolNames = new List<string>();

            foreach (Protocol protocol in Protocols)
            {
                protocolNames.Add(protocol.Name);
            }

            string newProtocolName = GetUniqueName(protocolNames, "Protocol");

            Protocol newProtocol = new Protocol(
                newProtocolName,
                AppDataFolderFileHelper.GenerateUnusedId(Protocols),
                packetMode);

            Protocols.Add(newProtocol);

            SelectedProtocol = newProtocol;
        }

        private void ProtocolDelete()
        {
            if (SelectedProtocol != null)
            {
                if (SelectedProtocol.CanNotDelete)
                {
                    ShowErrorDialog.ShowError("Error", $"The {SelectedProtocol.Name} Protocol can not be deleted.");
                }
                else
                {
                    Protocols.Remove(SelectedProtocol);
                }
            }
        }

        private void FieldUp()
        {
            if (SelectedProtocol != null && SelectedPacketField != null)
            {
                IList<PacketField>? fields = SelectedProtocol.PacketMode.Fields;
                int index = SelectedPacketFieldIndex;

                if (index > 0)
                {
                    PacketField selectedPacketFieldPrevious = fields[index - 1];
                    fields.RemoveAt(index - 1);
                    fields.Insert(index, selectedPacketFieldPrevious);
                }
            }
        }

        private void FieldDown()
        {
            if (SelectedProtocol != null && SelectedPacketField != null)
            {
                IList<PacketField>? fields = SelectedProtocol.PacketMode.Fields;
                int index = SelectedPacketFieldIndex;

                if (index < fields.Count - 1)
                {
                    PacketField selectedPacketFieldNext = fields[index + 1];
                    fields.RemoveAt(index + 1);
                    fields.Insert(index, selectedPacketFieldNext);
                }
            }
        }

        private void FieldNew(Type? type)
        {
            if (type == null)
            {
                ShowErrorDialog.ShowError("Error", "FieldNew with null type");
            }
            else
            {
                if (SelectedProtocol != null)
                {
                    IList<PacketField>? fields = SelectedProtocol.PacketMode.Fields;
                    int selectedNextIndex = fields.Count;
                    if (SelectedPacketField != null)
                    {
                        selectedNextIndex = SelectedPacketFieldIndex + 1;
                    }

                    PacketField? newPacketField = null;
                    if (type == typeof(PacketField))
                    {
                        newPacketField = new PacketField(
                            "Data",
                            false,
                            Array.Empty<byte>(),
                            0);
                        fields.Insert(selectedNextIndex, newPacketField);
                    }
                    else if (type == typeof(EndOfPacketSymbol))
                    {
                        newPacketField = new EndOfPacketSymbol(
                            "EOP",
                            [(byte)'\r', (byte)'\n']);
                        fields.Insert(fields.Count, newPacketField);
                    }
                    else if (type == typeof(Preamble))
                    {
                        newPacketField = new Preamble(
                            "Preamble",
                            [0x00]);
                        fields.Insert(0, newPacketField);
                    }

                    if (newPacketField != null)
                    {
                        List<string> fieldNames = new List<string>();
                        foreach (var field in fields)
                        {
                            fieldNames.Add(field.Name);
                        }

                        newPacketField.Name = GetUniqueName(fieldNames, "Field");
                        newPacketField.FixedLength = 1;

                        SelectedPacketField = newPacketField;
                    }
                }
            }
        }

        private string GetUniqueName(IList<string> currentList, string prefix)
        {
            HashSet<string> table = new HashSet<string>();
            foreach (string s in currentList)
            {
                if (s.StartsWith(prefix))
                {
                    table.Add(s);
                }
            }

            int numberNow = 0;
            while (true)
            {
                ++numberNow;
                string name = $"{prefix}{numberNow}";
                if (!table.Contains(name))
                {
                    break;
                }
            }

            return $"{prefix}{numberNow}";
        }

        private void FieldDelete()
        {
            if (SelectedProtocol != null && SelectedPacketField != null)
            {
                IList<PacketField>? fields = SelectedProtocol.PacketMode.Fields;
                fields.RemoveAt(SelectedPacketFieldIndex);
            }
        }

        private void FieldValidate()
        {
            try
            {
                ValidateResult = "N/A";

                SelectedPacketField = null;

                SelectedProtocol?.PacketMode.Validate();

                ValidateResult = "OK";
            }
            catch (Exception ex)
            {
                ValidateResult = "Invalid";
                ShowErrorDialog.ShowError("Invalid Protocol", ex.Message);
            }
        }
    }
}
