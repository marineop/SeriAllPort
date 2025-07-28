using CommonWpf;
using CommonWpf.ViewModels.ListEditor;
using System.Collections.ObjectModel;

namespace SeriAllPort.ViewModels.Commands
{
    public class CommandListEditor : ListEditorViewModel<CommandEditor>
    {
        public ObservableCollection<CommandEditor> CommandViewModels => Items;

        public CommandEditor? SelectedCommandViewModel
        {
            get => SelectedItem;
            set => SelectedItem = value;
        }

        public override string Name => "Commands";

        public override ObservableCollection<Tuple<string, object>> NewTypes =>
        [
           Tuple.Create<string,object>("No Protocol Command", 0)
        ];

        public CommandListEditor(IList<CommandEditor> commandViewModels,
                                 int selectedIndex,
                                 IShowErrorDialog showErrorDialog)
                : base(commandViewModels, selectedIndex, showErrorDialog)
        {
        }

        public override void ItemNew(object? parameter)
        {
            if (parameter != null && parameter is int id)
            {
                if (id == 0)
                {
                    NoProtocolCommand noProtocolCommand = new NoProtocolCommand();
                    string newProtocolName = NameHelper.GetUniqueName(CommandViewModels, "Command");
                    noProtocolCommand.Name = newProtocolName;
                    CommandEditor commandEditorViewModel = new CommandEditor(noProtocolCommand, ShowErrorDialog);

                    CommandViewModels.Add(commandEditorViewModel);

                    SelectedCommandViewModel = commandEditorViewModel;
                }
            }
        }
    }
}
