using CommonWpf;
using CommonWpf.ViewModels.ListEditor;

namespace SeriAllPort.ViewModels.Commands
{
    public class CommandEditorViewModel : ViewModel, IListEditorItem
    {
        public string TypeName => Command.TypeName;

        public string Name
        {
            get => Command.Name;
            set
            {
                if (Command.Name != value)
                {
                    Command.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool CanNotDelete => false;

        private Command _command;
        public Command Command
        {
            get => _command;
            set
            {
                if (_command != value)
                {
                    _command = value;
                    OnPropertyChanged();
                }
            }
        }

        protected IShowErrorDialog ShowErrorDialog { get; set; }

        public CommandEditorViewModel(Command command, IShowErrorDialog showErrorDialog)
        {
            _command = command;
            ShowErrorDialog = showErrorDialog;
        }
    }
}
