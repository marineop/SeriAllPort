using CommonWpf;
using CommonWpf.Communication.Protocol;
using CommonWpf.Communication.Protocol.PacketModes;
using CommonWpf.FileHelper;
using CommonWpf.ViewModels.ListEditor;
using System.Collections.ObjectModel;

namespace SeriAllPort.ViewModels.Protocols
{
    public class ProtocolListEditorViewModel : ListEditorViewModel<ProtocolEditorViewModel>
    {
        public ObservableCollection<ProtocolEditorViewModel> ProtocolViewModels => Items;

        public ProtocolEditorViewModel? SelectedProtocolViewModel
        {
            get => SelectedItem;
            set => SelectedItem = value;
        }

        public override string Name => "Protocols";

        public override ObservableCollection<Tuple<string, object>> NewTypes =>
        [
           Tuple.Create<string,object>("Protocol", 0)
        ];

        public ProtocolListEditorViewModel(IList<ProtocolEditorViewModel> protocols,
                                       int selectedIndex,
                                       IShowErrorDialog showErrorDialog)
            : base(protocols, selectedIndex, showErrorDialog)
        {
        }

        public override void ItemNew(object? parameter)
        {
            PacketModeTimeout packetMode = new PacketModeTimeout(0);

            List<string> protocolNames = [];

            foreach (ProtocolEditorViewModel protocolViewModel in ProtocolViewModels)
            {
                protocolNames.Add(protocolViewModel.Name);
            }

            string newProtocolName = GetUniqueName(protocolNames, "Protocol");

            Protocol newProtocol = new Protocol(
                newProtocolName,
                AppDataFolderFileHelper.GenerateUnusedId(ProtocolViewModels),
                packetMode);

            ProtocolEditorViewModel newProtocolEditorViewModel = new ProtocolEditorViewModel(newProtocol, ShowErrorDialog);

            ProtocolViewModels.Add(newProtocolEditorViewModel);

            SelectedProtocolViewModel = newProtocolEditorViewModel;
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
