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

            string newProtocolName = NameHelper.GetUniqueName(ProtocolViewModels, "Protocol");

            Protocol newProtocol = new Protocol(
                newProtocolName,
                AppDataFolderFileHelper.GenerateUnusedId(ProtocolViewModels),
                packetMode);

            ProtocolEditorViewModel newProtocolEditorViewModel = new ProtocolEditorViewModel(newProtocol, ShowErrorDialog);

            ProtocolViewModels.Add(newProtocolEditorViewModel);

            SelectedProtocolViewModel = newProtocolEditorViewModel;
        }
    }
}
