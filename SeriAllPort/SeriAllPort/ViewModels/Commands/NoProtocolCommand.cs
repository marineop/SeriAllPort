using CommonWpf.ViewModels.TextBytes;
using System.Text.Json.Serialization;

namespace SeriAllPort.ViewModels.Commands
{
    public class NoProtocolCommand : Command
    {
        [JsonIgnore]
        public override string TypeName => "No Protocol";

        private TextBytes _textBytes = new();
        public TextBytes TextBytes
        {
            get => _textBytes;
            set
            {
                if (_textBytes != value)
                {
                    _textBytes = value;
                    OnPropertyChanged();
                }
            }
        }

        public NoProtocolCommand()
        {
            TextBytes.SetTextWithCurrentBytes();

        }

        public override Command CreateClone()
        {
            NoProtocolCommand command = new NoProtocolCommand();

            command.TextBytes = TextBytes.CreateClone();

            AssignWithSelfValue(command);

            return command;
        }

        public override byte[] GetBytes()
        {
            return TextBytes.Bytes;
        }
    }
}
