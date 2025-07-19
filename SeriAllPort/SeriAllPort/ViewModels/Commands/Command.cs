using CommonWpf;
using System.Text.Json.Serialization;

namespace SeriAllPort.ViewModels.Commands
{
    [JsonDerivedType(typeof(NoProtocolCommand), typeDiscriminator: "NoProtocol")]
    public abstract class Command : ViewModel
    {
        public abstract string TypeName { get; }

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

        public abstract byte[] GetBytes();

        public abstract Command CreateClone();

        protected void AssignWithSelfValue(Command command)
        {
            command.Name = Name;
        }
    }
}
