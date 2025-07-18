﻿using CommonWpf.ViewModels.TextBytes;
using System.Text.Json.Serialization;

namespace CommonWpf.Communication.Protocol.PacketFields
{
    [JsonDerivedType(typeof(PacketField), typeDiscriminator: "PacketField")]
    [JsonDerivedType(typeof(EndOfPacketSymbol), typeDiscriminator: "EOP")]
    [JsonDerivedType(typeof(Preamble), typeDiscriminator: "Preamble")]
    public class PacketField : ViewModel
    {
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

        private LengthMode _lengthMode = LengthMode.FixedLength;
        public virtual LengthMode LengthMode
        {
            get => _lengthMode;
            set
            {
                if (_lengthMode != value)
                {
                    _lengthMode = value;

                    byte[] newBytes = TextBytes.Bytes;

                    if (_lengthMode == LengthMode.FixedLength)
                    {
                        newBytes = [];
                        if (FixedLength <= 1)
                        {
                            FixedLength = 1;
                        }
                    }
                    else if (_lengthMode == LengthMode.FixedData)
                    {
                        if (newBytes == null || newBytes.Length < 1)
                        {
                            newBytes = new byte[1];
                        }
                    }
                    else
                    {
                        FixedLength = 0;
                        newBytes = [];
                    }

                    TextBytes.Bytes = newBytes;
                    TextBytes.SetTextWithCurrentBytes();

                    OnPropertyChanged();
                }
            }
        }

        private TextBytesViewModel _textBytes = new();
        public TextBytesViewModel TextBytes
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

        [JsonIgnore]
        public byte[] Data
        {
            get => TextBytes.Bytes;
            set => TextBytes.Bytes = value;
        }

        private int _fixedLength = 0;
        public int FixedLength
        {
            get => _fixedLength;
            set
            {
                if (_fixedLength != value)
                {
                    int newValue = value;

                    if (LengthMode == LengthMode.FixedLength)
                    {
                        if (newValue <= 0)
                        {
                            newValue = 1;
                        }
                    }
                    else if (LengthMode == LengthMode.FixedData)
                    {
                        newValue = TextBytes.Bytes.Length;
                    }
                    else
                    {
                        newValue = 0;
                    }

                    _fixedLength = newValue;
                    OnPropertyChanged();
                }
            }
        }

        private object? _value = null;
        [JsonIgnore]
        public object? Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged();
                }
            }
        }

        [JsonIgnore]
        public virtual string TypeName { get; } = "Field";

        public PacketField(string name, LengthMode lengthMode, TextBytesViewModel textBytes, int fixedLength)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            LengthMode = lengthMode;

            TextBytes = textBytes ?? throw new ArgumentNullException(nameof(textBytes));
            TextBytes.SetTextWithCurrentBytes();
            TextBytes.PreviewUpdateBytesHook += UpdateBytesCheck;
            TextBytes.PostUpdateBytesHook += BytesUpdated;

            FixedLength = fixedLength;
        }

        public virtual PacketField CreateClone()
        {
            TextBytesViewModel newTextBytes = TextBytes.CreateClone();
            newTextBytes.SetTextWithCurrentBytes();

            PacketField newPacketField = new PacketField(
                Name,
                LengthMode,
                newTextBytes,
                FixedLength);

            return newPacketField;
        }

        protected void AssignWithSelfValue(PacketField packetField)
        {
            packetField.Name = Name;

            packetField.TextBytes = TextBytes.CreateClone();
            packetField.TextBytes.SetTextWithCurrentBytes();
            packetField.TextBytes.PreviewUpdateBytesHook += UpdateBytesCheck;
            packetField.TextBytes.PostUpdateBytesHook += BytesUpdated;

            packetField.FixedLength = FixedLength;
            packetField.Value = Value;
        }

        internal void RefreshValues()
        {
            TextBytes.SetTextWithCurrentBytes();
            OnPropertyChanged(nameof(FixedLength));
        }

        private byte[] UpdateBytesCheck(byte[] newData)
        {
            byte[] answer = [];

            if (LengthMode == LengthMode.FixedData)
            {
                if (newData == null)
                {
                    throw new Exception("Invalid Data");
                }

                if (newData.Length <= 0)
                {
                    newData = new byte[1];
                }

                answer = newData;
            }

            return answer;
        }

        private void BytesUpdated()
        {
            FixedLength = TextBytes.Bytes.Length;
        }
    }
}
