using CommonWpf;
using CommonWpf.ViewModels.ListEditor;
using System.Collections.ObjectModel;

namespace SeriAllPort.ViewModels.Commands
{
    public class CommandListEditorViewModel : ListEditorViewModel<CommandEditorViewModel>
    {
        public ObservableCollection<CommandEditorViewModel> CommandViewModels => Items;

        public CommandEditorViewModel? SelectedCommandViewModel
        {
            get => SelectedItem;
            set => SelectedItem = value;
        }

        public override string Name => "Commands";

        public override ObservableCollection<Tuple<string, object>> NewTypes =>
        [
           Tuple.Create<string,object>("No Protocol Command", 0)
        ];

        public CommandListEditorViewModel(IList<CommandEditorViewModel> commandViewModels,
                               int selectedIndex,
                               IShowErrorDialog showErrorDialog)
                : base(commandViewModels, selectedIndex, showErrorDialog)
        {
        }

        public override void ItemNew(object? parameter)
        {
            List<string> names = [];
            if (parameter != null && parameter is int)
            {
                string newProtocolName = NameHelper.GetUniqueName(CommandViewModels, "Command");

                int id = (int)parameter;

                if (id == 0)
                {
                    NoProtocolCommand noProtocolCommand = new NoProtocolCommand();
                    noProtocolCommand.Name = newProtocolName;
                    CommandEditorViewModel commandEditorViewModel = new CommandEditorViewModel(noProtocolCommand, ShowErrorDialog);

                    CommandViewModels.Add(commandEditorViewModel);

                    SelectedCommandViewModel = commandEditorViewModel;
                }
            }
        }
    }
}
