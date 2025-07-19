using CommonWpf.Extensions;
using System.Text;
using System.Text.Json.Serialization;

namespace CommonWpf.ViewModels.TextBytes
{
    public class TextBytesViewModel : NotifyErrorViewModel
    {
        private TextRepresentation _textRepresentation;
        public TextRepresentation TextRepresentation
        {
            get => _textRepresentation;
            set
            {
                if (_textRepresentation != value)
                {
                    _textRepresentation = value;

                    OnPropertyChanged();

                    ValidateText();
                }
            }
        }

        private string _text = string.Empty;
        [JsonIgnore]
        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged();

                    ValidateText();
                }
            }
        }

        private byte[] _bytes = [];
        public byte[] Bytes
        {
            get => _bytes;
            set
            {
                if (_bytes != value)
                {
                    _bytes = value;
                    OnPropertyChanged();
                }
            }
        }

        [JsonIgnore]
        public Func<byte[], byte[]>? PreviewUpdateBytesHook { get; set; }

        [JsonIgnore]
        public Action? PostUpdateBytesHook { get; set; }

        public TextBytesViewModel()
        {
            TextRepresentation = TextRepresentation.Bytes;
            Bytes = [];
        }

        public TextBytesViewModel(TextRepresentation textRepresentation, byte[] bytes)
        {
            TextRepresentation = textRepresentation;
            Bytes = bytes;
        }

        public TextBytesViewModel CreateClone()
        {
            TextBytesViewModel copy = new TextBytesViewModel();

            copy.TextRepresentation = TextRepresentation;
            copy.Text = Text;
            copy.Bytes = Bytes;

            copy.SetTextWithCurrentBytes();
            copy.PreviewUpdateBytesHook = PreviewUpdateBytesHook;
            copy.PostUpdateBytesHook = PostUpdateBytesHook;

            return copy;
        }

        public void SetTextWithCurrentBytes()
        {
            if (TextRepresentation == TextRepresentation.Text)
            {
                Text = Encoding.UTF8.GetString(Bytes);
            }
            else
            {
                Text = Bytes.BytesToString();
            }
        }

        private void ValidateText()
        {
            ClearErrors(nameof(Text));

            bool valid = true;
            if (TextRepresentation == TextRepresentation.Bytes)
            {
                foreach (char c in Text)
                {
                    if (!char.IsWhiteSpace(c) && !c.IsHexDigit())
                    {
                        valid = false;
                        AddError(nameof(Text), $"Invalid character: {c}");
                    }
                }
            }

            if (valid)
            {
                try
                {
                    byte[] newBytes = [];
                    if (TextRepresentation == TextRepresentation.Bytes)
                    {
                        newBytes = _text.HexStringToBytes();
                    }
                    else if (TextRepresentation == TextRepresentation.Text)
                    {
                        newBytes = Encoding.UTF8.GetBytes(_text);
                    }

                    Bytes = PreviewUpdateBytesHook?.Invoke(newBytes) ?? newBytes;

                    PostUpdateBytesHook?.Invoke();
                }
                catch (Exception ex)
                {
                    AddError(nameof(Text), ex.Message);
                }
            }
        }
    }
}
