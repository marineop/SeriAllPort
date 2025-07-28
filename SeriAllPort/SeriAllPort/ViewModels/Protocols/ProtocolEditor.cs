using CommonWpf;
using CommonWpf.Communication.Protocol;
using CommonWpf.Communication.Protocol.PacketFields;
using CommonWpf.Communication.Protocol.PacketModes;
using CommonWpf.FileHelper;
using CommonWpf.ViewModels.ListEditor;
using CommonWpf.ViewModels.TextBytes;
using System.Collections.ObjectModel;

namespace SeriAllPort.ViewModels.Protocols
{
    public class ProtocolEditor : ViewModel, IListEditorItem, IAppDataFolderFile
    {
        public string Name
        {
            get => Protocol.Name;
            set
            {
                if (Protocol.Name != value)
                {
                    Protocol.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool CanNotDelete => Protocol.CanNotDelete;

        public static string AppDataSubFolder => Protocol.AppDataSubFolder;

        public static string FileExtensionName => Protocol.FileExtensionName;

        public Guid Id
        {
            get => Protocol.Id;
            set => Protocol.Id = value;
        }

        public int Order
        {
            get => Protocol.Order;
            set => Protocol.Order = value;
        }

        private Protocol _protocol;
        public Protocol Protocol
        {
            get => _protocol;
            set
            {
                if (_protocol != value)
                {
                    _protocol = value;
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

        private ObservableCollection<PacketMode> _packetModes = [];
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

        protected IShowErrorDialog ShowErrorDialog { get; set; }

        public SimpleCommand FieldUpCommand { get; private set; }
        public SimpleCommand FieldDownCommand { get; private set; }
        public SimpleCommand FieldNewCommand { get; private set; }
        public SimpleCommand FieldDeleteCommand { get; private set; }
        public SimpleCommand FieldValidateCommand { get; private set; }

        public ProtocolEditor(Protocol protocol, IShowErrorDialog showErrorDialog)
        {
            _protocol = protocol;
            ShowErrorDialog = showErrorDialog;

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
                (parameter) => FieldValidate());

            PacketModes =
            [

                PacketModeTimeout.CreateDefault(),
                new PacketModeEndOfPacketSymbol([(byte)'\r', (byte)'\n']),
                PacketModeLengthField.CreateDefault(),
                Protocol.PacketMode,
            ];

            for (int i = 0; i < PacketModes.Count; ++i)
            {
                if (PacketModes[i].GetType() == Protocol.PacketMode.GetType())
                {
                    PacketModes.RemoveAt(i);
                    break;
                }
            }
        }

        private void FieldUp()
        {
            if (SelectedPacketField != null)
            {
                ObservableCollection<PacketField> fields = Protocol.PacketMode.Fields;
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
            if (Protocol != null && SelectedPacketField != null)
            {
                ObservableCollection<PacketField> fields = Protocol.PacketMode.Fields;
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
                if (Protocol != null)
                {
                    ObservableCollection<PacketField> fields = Protocol.PacketMode.Fields;
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
                            LengthMode.FixedLength,
                            new TextBytes(),
                            1);
                        fields.Insert(selectedNextIndex, newPacketField);
                    }
                    else if (type == typeof(EndOfPacketSymbol))
                    {
                        newPacketField = new EndOfPacketSymbol(
                            "EOP",
                            new TextBytes(TextRepresentation.Bytes, [(byte)'\r', (byte)'\n']));
                        fields.Insert(fields.Count, newPacketField);
                    }
                    else if (type == typeof(Preamble))
                    {
                        newPacketField = new Preamble(
                            "Preamble",
                             new TextBytes(TextRepresentation.Bytes, [0x00]));
                        fields.Insert(0, newPacketField);
                    }
                    else if (type == typeof(LengthField))
                    {
                        newPacketField = new LengthField(
                            "Length Field",
                             1,
                             0,
                             Protocol.PacketMode.Fields.Count >= 1 ? Protocol.PacketMode.Fields.Count - 1 : 0,
                             0);
                        fields.Insert(selectedNextIndex, newPacketField);
                    }

                    if (newPacketField != null)
                    {
                        List<string> fieldNames = [];
                        foreach (PacketField field in fields)
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

        private void FieldDelete()
        {
            if (Protocol != null && SelectedPacketField != null)
            {
                ObservableCollection<PacketField> fields = Protocol.PacketMode.Fields;
                fields.RemoveAt(SelectedPacketFieldIndex);
            }
        }

        private void FieldValidate()
        {
            try
            {
                ValidateResult = "N/A";

                SelectedPacketField = null;

                Protocol?.PacketMode.Validate();

                ValidateResult = "OK";
            }
            catch (Exception ex)
            {
                ValidateResult = "Invalid";
                ShowErrorDialog.ShowError("Invalid Protocol", ex.Message);
            }
        }

        private static string GetUniqueName(IList<string> currentList, string prefix)
        {
            HashSet<string> table = [];
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
    }
}
